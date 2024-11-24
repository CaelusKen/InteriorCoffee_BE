using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Enums.Product;
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
using System.Text.Json;
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

        #region Utility Function
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
        #endregion

        public async Task<ProductResponseDTO> GetProductsAsync(
            int? pageNo, int? pageSize, decimal? minPrice, decimal? maxPrice, OrderBy orderBy, ProductFilterDTO filter, string keyword = null)
        {
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

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? products.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)products.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered products
                var paginatedProducts = products.Skip((paginationPageNo - 1) * finalPageSize)
                                                .Take(finalPageSize)
                                                .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedProducts.Count;

                var inStockCount = products.Count(p => p.Quantity > 0);
                var outOfStockCount = products.Count(p => p.Quantity == 0);

                var productResponseItems = _mapper.Map<List<ProductResponseItemDTO>>(paginatedProducts);

                #region "Mapping"
                return new ProductResponseDTO
                {
                    PageNo = paginationPageNo,
                    PageSize = finalPageSize,
                    ListSize = totalItems,
                    CurrentPageSize = listAfter,
                    ListSizeAfter = listAfter,
                    TotalPage = totalPages,
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
                _logger.LogError(ex, "Error occurred while getting products.");
                return new ProductResponseDTO
                {
                    PageNo = 1,
                    PageSize = 0,
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
            foreach (var categoryId in createProductDTO.CategoryIds)
            {
                if (!await _productCategoryRepository.CategoryExistsAsync(categoryId))
                {
                    throw new NotFoundException($"Category with id {categoryId} not found.");
                }
            }

            var product = _mapper.Map<Product>(createProductDTO);
            await _productRepository.CreateProductAsync(product);
        }

        public async Task UpdateProductAsync(string id, JsonElement updateProduct)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            // Log existing product details
            _logger.LogInformation("Existing product before update: {existingProduct}", existingProduct);

            var existingProductJson = JsonSerializer.Serialize(existingProduct, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var existingProductElement = JsonDocument.Parse(existingProductJson).RootElement;

            var mergedProduct = JsonUtil.MergeJsonElements(existingProductElement, updateProduct);

            var jsonString = JsonSerializer.Serialize(mergedProduct, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogInformation("Merged product JSON: {jsonString}", jsonString);

            var updateProductDto = JsonSerializer.Deserialize<UpdateProductDTO>(jsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Validate category IDs
            foreach (var categoryId in updateProductDto.CategoryIds)
            {
                if (!await _productCategoryRepository.CategoryExistsAsync(categoryId))
                {
                    throw new NotFoundException($"Category with id {categoryId} not found.");
                }
            }

            // Preserve the existing _id before mapping
            var existingId = existingProduct._id;

            // Map the updated fields to the existing product, excluding _id
            _mapper.Map(updateProductDto, existingProduct);

            // Ensure the _id is preserved
            existingProduct._id = existingId;

            // Log updated product details
            _logger.LogInformation("Updated product after mapping: {existingProduct}", existingProduct);

            await _productRepository.UpdateProductAsync(id, existingProduct);

            // Log updated product from repository
            var updatedProduct = await _productRepository.GetProductByIdAsync(id);
            _logger.LogInformation("Product after update from repository: {updatedProduct}", updatedProduct);
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


