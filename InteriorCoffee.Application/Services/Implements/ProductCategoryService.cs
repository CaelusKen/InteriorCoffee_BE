using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.ProductCategory;
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
    public class ProductCategoryService : BaseService<ProductCategoryService>, IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(ILogger<ProductCategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IProductCategoryRepository productCategoryRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<(List<ProductCategory>, int, int, int, int)> GetProductCategoriesAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allProductCategories, totalItems) = await _productCategoryRepository.GetProductCategoriesAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var productCategories = allProductCategories.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                                            .Take(pagination.PageSize)
                                                            .ToList();

                return (productCategories, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated product categories.");
                return (new List<ProductCategory>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }

        public async Task<ProductCategory> GetProductCategoryByIdAsync(string id)
        {
            var productCategory = await _productCategoryRepository.GetProductCategoryById(id);
            if (productCategory == null)
            {
                throw new NotFoundException($"Product category with id {id} not found.");
            }
            return productCategory;
        }

        public async Task CreateProductCategoryAsync(CreateProductCategoryDTO createProductCategoryDTO)
        {
            var productCategory = _mapper.Map<ProductCategory>(createProductCategoryDTO);
            await _productCategoryRepository.CreateProductCategory(productCategory);
        }

        public async Task UpdateProductCategoryAsync(string id, UpdateProductCategoryDTO updateProductCategoryDTO)
        {
            var existingProductCategory = await _productCategoryRepository.GetProductCategoryById(id);
            if (existingProductCategory == null)
            {
                throw new NotFoundException($"Product category with id {id} not found.");
            }
            _mapper.Map(updateProductCategoryDTO, existingProductCategory);
            await _productCategoryRepository.UpdateProductCategory(existingProductCategory);
        }

        public async Task DeleteProductCategoryAsync(string id)
        {
            var productCategory = await _productCategoryRepository.GetProductCategoryById(id);
            if (productCategory == null)
            {
                throw new NotFoundException($"Product category with id {id} not found.");
            }
            await _productCategoryRepository.DeleteProductCategory(id);
        }
    }
}
