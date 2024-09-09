using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product ID cannot be null or empty.");
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            if (product == null) throw new ArgumentException("Product cannot be null.");
            await _productRepository.CreateProductAsync(product);
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            if (string.IsNullOrEmpty(id) || product == null) throw new ArgumentException("Product ID and data cannot be null or empty.");
            await _productRepository.UpdateProductAsync(id, product);
        }

        public async Task DeleteProductAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Product ID cannot be null or empty.");
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
