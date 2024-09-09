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
            return await _productCategoryRepository.GetProductCategoryList();
        }

        public async Task<ProductCategory> GetProductCategoryByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product category ID cannot be null or empty.");
            return await _productCategoryRepository.GetProductCategoryById(id);
        }

        public async Task CreateProductCategoryAsync(ProductCategory productCategory)
        {
            if (productCategory == null) throw new ArgumentException("Product category cannot be null.");
            await _productCategoryRepository.CreateProductCategory(productCategory);
        }

        public async Task UpdateProductCategoryAsync(string id, ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(id) || productCategory == null)
                throw new ArgumentException("Product category ID and data cannot be null or empty.");
            await _productCategoryRepository.UpdateProductCategory(productCategory);
        }

        public async Task DeleteProductCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product category ID cannot be null or empty.");
            await _productCategoryRepository.DeleteProductCategory(id);
        }
    }
}
