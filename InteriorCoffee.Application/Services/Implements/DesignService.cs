using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Application.DTOs.OrderBy;
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
    public class DesignService : BaseService<DesignService>, IDesignService
    {
        private readonly IDesignRepository _designRepository;

        public DesignService(ILogger<DesignService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IDesignRepository designRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _designRepository = designRepository;
        }

        public async Task<(List<Design>, int, int, int, int)> GetDesignsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allDesigns, totalItems) = await _designRepository.GetDesignsAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var designs = allDesigns.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                        .Take(pagination.PageSize)
                                        .ToList();

                return (designs, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated designs.");
                return (new List<Design>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }


        public async Task<Design> GetDesignByIdAsync(string id)
        {
            var design = await _designRepository.GetDesignById(id);
            if (design == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }
            return design;
        }

        public async Task CreateDesignAsync(CreateDesignDTO createDesignDTO)
        {
            var design = _mapper.Map<Design>(createDesignDTO);
            await _designRepository.CreateDesign(design);
        }

        public async Task UpdateDesignAsync(string id, UpdateDesignDTO updateDesignDTO)
        {
            var existingDesign = await _designRepository.GetDesignById(id);
            if (existingDesign == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }
            _mapper.Map(updateDesignDTO, existingDesign);
            await _designRepository.UpdateDesign(existingDesign);
        }

        public async Task DeleteDesignAsync(string id)
        {
            var design = await _designRepository.GetDesignById(id);
            if (design == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }
            await _designRepository.DeleteDesign(id);
        }
    }
}
