using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Merchant;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Enums.Merchant;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class MerchantService : BaseService<MerchantService>, IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IAccountRepository _accountRepository;

        public MerchantService(ILogger<MerchantService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMerchantRepository merchantRepository, IAccountRepository accountRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
            _accountRepository = accountRepository;
        }

        #region "Dictionary"
        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "name", "Name" },
            { "email", "Email" },
            { "status", "Status" }
        };
        #endregion


        #region "Sorting"
        private List<Merchant> ApplySorting(List<Merchant> merchants, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Merchant).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        merchants = orderBy.Ascending
                            ? merchants.OrderBy(m => propertyInfo.GetValue(m, null)).ToList()
                            : merchants.OrderByDescending(m => propertyInfo.GetValue(m, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Merchant'.");
                }
            }
            return merchants;
        }
        #endregion

        #region "Filtering"
        private List<Merchant> ApplyFilters(List<Merchant> merchants, string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                merchants = merchants.Where(m => m.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return merchants;
        }
        #endregion

        public async Task<MerchantResponseDTO> GetMerchantsAsync(int? pageNo, int? pageSize, OrderBy orderBy, MerchantFilterDTO filter, string keyword)
        {
            try
            {
                var (allMerchants, totalItems) = await _merchantRepository.GetMerchantsAsync();

                // Apply filters
                allMerchants = ApplyFilters(allMerchants, filter.Status);

                // Apply keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    allMerchants = allMerchants.Where(m => m.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                           m.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                               .ToList();
                }

                // Apply sorting logic only if orderBy is not null
                allMerchants = ApplySorting(allMerchants, orderBy);

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? allMerchants.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)allMerchants.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered merchants
                var paginatedMerchants = allMerchants.Skip((paginationPageNo - 1) * finalPageSize)
                                                     .Take(finalPageSize)
                                                     .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedMerchants.Count;

                var merchantResponseItems = _mapper.Map<List<MerchantResponseItemDTO>>(paginatedMerchants);

                #region "Mapping"
                return new MerchantResponseDTO
                {
                    PageNo = paginationPageNo,
                    PageSize = finalPageSize,
                    ListSize = totalItems,
                    CurrentPageSize = listAfter,
                    ListSizeAfter = listAfter,
                    TotalPage = totalPages,
                    OrderBy = new MerchantOrderByDTO
                    {
                        SortBy = orderBy?.SortBy,
                        IsAscending = orderBy?.Ascending ?? true
                    },
                    Filter = new MerchantFilterDTO
                    {
                        Status = filter.Status
                    },
                    Keyword = keyword,
                    Merchants = merchantResponseItems
                };
                #endregion
            }
            #region "Catch error"
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting merchants.");
                return new MerchantResponseDTO
                {
                    PageNo = 1,
                    PageSize = 0,
                    ListSize = 0,
                    CurrentPageSize = 0,
                    ListSizeAfter = 0,
                    TotalPage = 0,
                    OrderBy = new MerchantOrderByDTO(),
                    Filter = new MerchantFilterDTO(),
                    Keyword = keyword,
                    Merchants = new List<MerchantResponseItemDTO>()
                };
            }
            #endregion
        }


        public async Task<Merchant> GetMerchantByIdAsync(string id)
        {
            var merchant = await _merchantRepository.GetMerchantById(id);
            if (merchant == null)
            {
                throw new NotFoundException($"Merchant with id {id} not found.");
            }
            return merchant;
        }

        public async Task CreateMerchantAsync(CreateMerchantDTO createMerchantDTO)
        {
            var merchant = _mapper.Map<Merchant>(createMerchantDTO);
            await _merchantRepository.CreateMerchant(merchant);
        }

        public async Task UpdateMerchantAsync(string id, UpdateMerchantDTO updateMerchantDTO)
        {
            var existingMerchant = await _merchantRepository.GetMerchantById(id);
            if (existingMerchant == null)
            {
                throw new NotFoundException($"Merchant with id {id} not found.");
            }
            _mapper.Map(updateMerchantDTO, existingMerchant);
            await _merchantRepository.UpdateMerchant(existingMerchant);
        }

        public async Task DeleteMerchantAsync(string id)
        {
            var merchant = await _merchantRepository.GetMerchantById(id);
            if (merchant == null)
            {
                throw new NotFoundException($"Merchant with id {id} not found.");
            }
            await _merchantRepository.DeleteMerchant(id);
        }

        public async Task VerifyMerchantAsync(string id)
        {
            var existingMerchant = await _merchantRepository.GetMerchantById(id);
            if (existingMerchant == null)
            {
                throw new NotFoundException($"Merchant with id {id} not found.");
            }
            existingMerchant.Status = MerchantStatusEnum.ACTIVE.ToString();
            await _merchantRepository.UpdateMerchant(existingMerchant);

            Account merchantAccount = await _accountRepository.GetAccount(
                predicate: acc => acc.MerchantId.Equals(id));
            merchantAccount.Status = AccountStatusEnum.ACTIVE.ToString();
            await _accountRepository.UpdateAccount(merchantAccount);
        }
    }
}
