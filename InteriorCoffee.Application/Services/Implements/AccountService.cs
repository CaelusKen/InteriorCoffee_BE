using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
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
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository, IRoleRepository roleRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

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

        public async Task<(List<Account>, int, int, int, int)> GetAccountsAsync(int? pageNo, int? pageSize, OrderBy orderBy)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allAccounts, totalItems) = await _accountRepository.GetAccountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var accounts = allAccounts.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                          .Take(pagination.PageSize)
                                          .ToList();

                // Apply sorting logic only if orderBy is not null
                accounts = ApplySorting(accounts, orderBy);

                return (accounts, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated accounts.");
                return (new List<Account>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
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

            // Get the role for the new account
            Role customerRole = await _roleRepository.GetRole(
                predicate: r => r.Name.Equals(AccountRoleEnum.CUSTOMER.ToString()));

            // Setup new account information
            Account newAccount = _mapper.Map<Account>(createAccountDTO);
            newAccount.RoleId = customerRole._id;

            // Create new account
            await _accountRepository.CreateAccount(newAccount);
        }

        public async Task UpdateAccountAsync(string id, UpdateAccountDTO updateAccountDTO)
        {
            var existingAccount = await _accountRepository.GetAccountById(id);
            if (existingAccount == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }

            // Validate role-id if provided
            if (updateAccountDTO.RoleId != null)
            {
                var role = await _roleRepository.GetRoleById(updateAccountDTO.RoleId);
                if (role == null)
                {
                    throw new NotFoundException($"Role with id {updateAccountDTO.RoleId} not found.");
                }
            }

            _mapper.Map(updateAccountDTO, existingAccount);
            await _accountRepository.UpdateAccount(existingAccount);

        }



        public async Task SoftDeleteAccountAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }

            account.Status = AccountStatusEnum.INACTIVE.ToString();
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
    }
}
