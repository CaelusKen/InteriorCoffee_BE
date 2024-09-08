using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ILogger<ProductCategoryService> _logger;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, ILogger<ProductCategoryService> logger)
        {
            _productCategoryRepository = productCategoryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync()
        {
            try
            {
                return await _productCategoryRepository.GetProductCategoryList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all product categories.");
                throw;
            }
        }

        public async Task<ProductCategory> GetProductCategoryByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid product category ID.");
                throw new ArgumentException("Product category ID cannot be null or empty.");
            }

            try
            {
                return await _productCategoryRepository.GetProductCategoryById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting product category with id {id}.");
                throw;
            }
        }

        public async Task CreateProductCategoryAsync(ProductCategory productCategory)
        {
            if (productCategory == null)
            {
                _logger.LogWarning("Invalid product category data.");
                throw new ArgumentException("Product category cannot be null.");
            }

            try
            {
                await _productCategoryRepository.CreateProductCategory(productCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product category.");
                throw;
            }
        }

        public async Task UpdateProductCategoryAsync(string id, ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(id) || productCategory == null)
            {
                _logger.LogWarning("Invalid product category ID or data.");
                throw new ArgumentException("Product category ID and data cannot be null or empty.");
            }

            try
            {
                await _productCategoryRepository.UpdateProductCategory(productCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product category with id {id}.");
                throw;
            }
        }

        public async Task DeleteProductCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid product category ID.");
                throw new ArgumentException("Product category ID cannot be null or empty.");
            }

            try
            {
                await _productCategoryRepository.DeleteProductCategory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product category with id {id}.");
                throw;
            }
        }
    }
}
