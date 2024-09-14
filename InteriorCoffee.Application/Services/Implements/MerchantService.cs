using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Merchant;
using InteriorCoffee.Application.DTOs.Pagination;
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

        public MerchantService(ILogger<MerchantService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMerchantRepository merchantRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<(List<Merchant>, int, int, int, int)> GetMerchantsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            var (merchants, totalItems, currentPageSize, totalPages) = await _merchantRepository.GetMerchantsAsync(pagination.PageNo, pagination.PageSize);
            return (merchants, pagination.PageNo, currentPageSize, totalItems, totalPages);
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
    }
}
