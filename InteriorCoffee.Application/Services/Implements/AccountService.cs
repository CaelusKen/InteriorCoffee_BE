using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Implements;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _accountRepository = accountRepository;
        }

        #region Utility Function
        #region "Dictionary"
        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "username", "UserName" },
            { "email", "Email" },
            { "createddate", "CreatedDate" },
            { "updatedate", "UpdatedDate" },
            { "status", "Status" }
        };
        #endregion

        #region "Sorting"
        private List<Account> ApplySorting(List<Account> accounts, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Account).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        accounts = orderBy.Ascending
                            ? accounts.OrderBy(a => propertyInfo.GetValue(a, null)).ToList()
                            : accounts.OrderByDescending(a => propertyInfo.GetValue(a, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Account'.");
                }
            }
            return accounts;
        }
        #endregion

        #region "Filtering"
        private List<Account> ApplyFilters(List<Account> accounts, string role, string status)
        {
            if (!string.IsNullOrEmpty(role))
            {
                accounts = accounts.Where(a => a.Role.Equals(role)).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                accounts = accounts.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return accounts;
        }
        #endregion
        #endregion

        public async Task<AccountResponseDTO> GetAccountsAsync(int? pageNo, int? pageSize, OrderBy orderBy, AccountFilterDTO filter, string keyword)
        {
            try
            {
                var (allAccounts, totalItems) = await _accountRepository.GetAccountAsync();

                // Apply filters
                allAccounts = ApplyFilters(allAccounts, filter.Role, filter.Status);

                // Apply keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    allAccounts = allAccounts.Where(a => a.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                         a.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                         a.PhoneNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                         a.Address.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                             .ToList();
                }

                // Apply sorting logic only if orderBy is not null
                allAccounts = ApplySorting(allAccounts, orderBy);

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? allAccounts.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)allAccounts.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered accounts
                var paginatedAccounts = allAccounts.Skip((paginationPageNo - 1) * finalPageSize)
                                                   .Take(finalPageSize)
                                                   .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedAccounts.Count;

                var accountResponseItems = _mapper.Map<List<AccountResponseItemDTO>>(paginatedAccounts);

                #region "Mapping"
                return new AccountResponseDTO
                {
                    PageNo = paginationPageNo,
                    PageSize = finalPageSize,
                    ListSize = totalItems,
                    CurrentPageSize = listAfter,
                    ListSizeAfter = listAfter,
                    TotalPage = totalPages,
                    OrderBy = new AccountOrderByDTO
                    {
                        SortBy = orderBy?.SortBy,
                        IsAscending = orderBy?.Ascending ?? true
                    },
                    Filter = new AccountFilterDTO
                    {
                        Status = filter.Status,
                        Role = filter.Role
                    },
                    Keyword = keyword,
                    Accounts = accountResponseItems
                };
                #endregion
            }
            #region "Catch error"
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting accounts.");
                return new AccountResponseDTO
                {
                    PageNo = 1,
                    PageSize = 0,
                    ListSize = 0,
                    CurrentPageSize = 0,
                    ListSizeAfter = 0,
                    TotalPage = 0,
                    OrderBy = new AccountOrderByDTO(),
                    Filter = new AccountFilterDTO(),
                    Keyword = keyword,
                    Accounts = new List<AccountResponseItemDTO>()
                };
            }
            #endregion
        }



        public async Task<Account> GetAccountByIdAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            return _mapper.Map<Account>(account);
        }

        public async Task CreateAccountAsync(CreateAccountDTO createAccountDTO)
        {
            // Check if an account with the given email already exists
            Account existingAccount = await _accountRepository.GetAccount(
                predicate: a => a.Email.Equals(createAccountDTO.Email));
            if (existingAccount != null) throw new ConflictException("Email has already existed");

            // Setup new account information
            Account newAccount = _mapper.Map<Account>(createAccountDTO);
            newAccount.Password = HashUtil.ToSHA256Hash(newAccount.Password, string.Empty);
            newAccount.Role = createAccountDTO.Role;

            // Create new account
            await _accountRepository.CreateAccount(newAccount);
        }

        public async Task UpdateAccountAsync(string id, JsonElement updateAccount)
        {
            var existingAccount = await _accountRepository.GetAccountById(id);
            if (existingAccount == null) throw new NotFoundException($"Account with id {id} not found.");

            // Log existing account details
            _logger.LogInformation("Existing account before update: {existingAccount}", existingAccount);

            var existingAccountJson = JsonSerializer.Serialize(existingAccount, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var existingAccountElement = JsonDocument.Parse(existingAccountJson).RootElement;

            var mergedAccount = JsonUtil.MergeJsonElements(existingAccountElement, updateAccount);

            var jsonString = JsonSerializer.Serialize(mergedAccount, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogInformation("Merged account JSON: {jsonString}", jsonString);

            var updateAccountDto = JsonSerializer.Deserialize<UpdateAccountDTO>(jsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Preserve the existing _id before mapping
            var existingId = existingAccount._id;

            // Map the updated fields to the existing account, excluding _id
            _mapper.Map(updateAccountDto, existingAccount);

            // Ensure the _id is preserved
            existingAccount._id = existingId;

            // Log updated account details
            _logger.LogInformation("Updated account after mapping: {existingAccount}", existingAccount);

            await _accountRepository.UpdateAccount(existingAccount);

            // Log updated account from repository
            var updatedAccount = await _accountRepository.GetAccountById(id);
            _logger.LogInformation("Account after update from repository: {updatedAccount}", updatedAccount);
        }


        public async Task SoftDeleteAccountAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            account.Status = AccountStatusEnum.INACTIVE.ToString();
            account.UpdatedDate = DateTime.Now;
            await _accountRepository.UpdateAccount(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            await _accountRepository.DeleteAccount(id);
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            var account = await _accountRepository.GetAccount(predicate: acc => acc.Email.Equals(email));
            if (account == null)
            {
                throw new NotFoundException($"Account with email: {email} not found.");
            }
            return _mapper.Map<Account>(account);
        }
    }
}
