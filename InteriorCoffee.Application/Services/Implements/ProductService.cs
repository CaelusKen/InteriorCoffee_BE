using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Enums.Product;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
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
    public class ProductService : BaseService<ProductService>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductService(ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        #region "Dictionary"
        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "price", "TruePrice" },
            { "name", "Name" },
            { "createddate", "CreatedDate" },
            { "updatedate", "UpdatedDate" },
            { "status", "Status" }
        };
        #endregion

        public async Task<ProductResponseDTO> GetProductsAsync(
            int? pageNo, int? pageSize, decimal? minPrice, decimal? maxPrice, OrderBy orderBy, ProductFilterDTO filter, string keyword = null)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (products, totalItems) = await _productRepository.GetProductsAsync();

                // Apply keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    products = products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                   p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                       .ToList();
                }

                // Apply filters
                products = ApplyFilters(products, filter, minPrice, maxPrice);

                // Determine the min and max prices from the filtered product list
                var actualMinPrice = products.Any() ? products.Min(p => (decimal)p.TruePrice) : 0;
                var actualMaxPrice = products.Any() ? products.Max(p => (decimal)p.TruePrice) : 0;

                // Set default price range if not provided
                minPrice = minPrice ?? actualMinPrice;
                maxPrice = maxPrice ?? actualMaxPrice;

                // Apply sorting
                products = ApplySorting(products, orderBy);

                // Calculate the total items and pages based on the filtered list
                var listAfter = products.Count;
                var filteredTotalPages = (int)Math.Ceiling((double)listAfter / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > filteredTotalPages) pagination.PageNo = filteredTotalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                // Paginate the filtered products
                var paginatedProducts = products.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                                .Take(pagination.PageSize)
                                                .ToList();

                var inStockCount = products.Count(p => p.Quantity > 0);
                var outOfStockCount = products.Count(p => p.Quantity == 0);

                #region "Mapping"
                var productResponseItems = _mapper.Map<List<ProductResponseItemDTO>>(paginatedProducts);

                // Create and return the response DTO
                return new ProductResponseDTO
                {
                    PageNo = pagination.PageNo,
                    PageSize = pagination.PageSize,
                    ListSize = totalItems,
                    CurrentPageSize = productResponseItems.Count,
                    ListSizeAfter = listAfter,
                    TotalPage = filteredTotalPages,
                    MinPrice = minPrice.Value,
                    MaxPrice = maxPrice.Value,
                    InStock = inStockCount,
                    OutOfStock = outOfStockCount,
                    OrderBy = new ProductOrderByDTO
                    {
                        SortBy = orderBy?.SortBy,
                        IsAscending = orderBy?.Ascending ?? true
                    },
                    Filter = new ProductFilterDTO
                    {
                        Status = filter.Status,
                        CategoryId = filter.CategoryId,
                        MerchantId = filter.MerchantId,
                        IsAvailability = filter.IsAvailability
                    },
                    Keyword = keyword,
                    Products = productResponseItems
                };
                #endregion
            }
            #region "Catch error"
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated products.");
                return new ProductResponseDTO
                {
                    PageNo = pagination.PageNo,
                    PageSize = pagination.PageSize,
                    ListSize = 0,
                    CurrentPageSize = 0,
                    ListSizeAfter = 0,
                    TotalPage = 0,
                    MinPrice = 0,
                    MaxPrice = 0,
                    InStock = 0,
                    OutOfStock = 0,
                    OrderBy = new ProductOrderByDTO(),
                    Filter = new ProductFilterDTO(),
                    Keyword = keyword,
                    Products = new List<ProductResponseItemDTO>()
                };
            }
            #endregion
        }

        #region "Filtering"
        private List<Product> ApplyFilters(List<Product> products, ProductFilterDTO filter, decimal? minPrice, decimal? maxPrice)
        {
            if (!string.IsNullOrEmpty(filter.Status))
            {
                var normalizedStatus = filter.Status.ToUpper();
                products = products.Where(p => p.Status.ToUpper() == normalizedStatus).ToList();
            }

            if (!string.IsNullOrEmpty(filter.CategoryId))
            {
                products = products.Where(p => p.CategoryIds != null && p.CategoryIds.Contains(filter.CategoryId)).ToList();
            }

            if (!string.IsNullOrEmpty(filter.MerchantId))
            {
                products = products.Where(p => p.MerchantId != null && p.MerchantId == filter.MerchantId).ToList();
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => (decimal)p.TruePrice >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => (decimal)p.TruePrice <= maxPrice.Value).ToList();
            }

            if (filter.IsAvailability == true)
            {
                products = products.Where(p => p.Quantity > 0).ToList();
            }
            else if (filter.IsAvailability == false)
            {
                products = products.Where(p => p.Quantity == 0).ToList();
            }

            return products;
        }
        #endregion

        #region "OrderBy/Sort"
        private List<Product> ApplySorting(List<Product> products, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Product).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        products = orderBy.Ascending
                            ? products.OrderBy(p => propertyInfo.GetValue(p, null)).ToList()
                            : products.OrderByDescending(p => propertyInfo.GetValue(p, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Product'.");
                }
            }

            return products;
        }
        #endregion

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }
            return product;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO)
        {
            foreach (var categoryId in createProductDTO.CategoryId)
            {
                if (!await _productCategoryRepository.CategoryExistsAsync(categoryId))
                {
                    throw new NotFoundException($"Category with id {categoryId} not found.");
                }
            }

            var product = _mapper.Map<Product>(createProductDTO);
            await _productRepository.CreateProductAsync(product);
        }

        public async Task UpdateProductAsync(string id, UpdateProductDTO updateProductDTO)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            foreach (var categoryId in updateProductDTO.CategoryId)
            {
                if (!await _productCategoryRepository.CategoryExistsAsync(categoryId))
                {
                    throw new NotFoundException($"Category with id {categoryId} not found.");
                }
            }

            _mapper.Map(updateProductDTO, existingProduct);
            await _productRepository.UpdateProductAsync(id, existingProduct);
        }

        public async Task SoftDeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            product.Status = ProductStatusEnum.INACTIVE.ToString();
            await _productRepository.UpdateProductAsync(id, product);
        }

        public async Task DeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
