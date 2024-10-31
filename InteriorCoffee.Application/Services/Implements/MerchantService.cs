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

        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "name", "Name" },
            { "email", "Email" },
            { "createddate", "CreatedDate" },
            { "updatedate", "UpdatedDate" },
            { "status", "Status" }
        };

        public async Task<(List<Merchant>, int, int, int, int)> GetMerchantsAsync(int? pageNo, int? pageSize, OrderBy orderBy)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allMerchants, totalItems) = await _merchantRepository.GetMerchantsAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var merchants = allMerchants.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                            .Take(pagination.PageSize)
                                            .ToList();

                // Apply sorting logic only if orderBy is provided
                if (orderBy != null)
                {
                    merchants = ApplySorting(merchants, orderBy);
                }

                return (merchants, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated merchants.");
                return (new List<Merchant>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }

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
