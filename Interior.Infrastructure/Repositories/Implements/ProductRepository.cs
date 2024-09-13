using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IMongoDatabase database, ILogger<ProductRepository> logger)
        {
            _products = database.GetCollection<Product>("Product");
            _logger = logger;
        }

        public async Task<(List<Product>, int, int, int)> GetProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _products.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var products = await _products.Find(product => true)
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Limit(pageSize)
                                              .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (products, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated products.");
                throw;
            }
        }


        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                return await _products.Find<Product>(product => product._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting product with id {id}.");
                throw;
            }
        }

        public async Task CreateProductAsync(Product product)
        {
            try
            {
                await _products.InsertOneAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product.");
                throw;
            }
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            try
            {
                await _products.ReplaceOneAsync(prod => prod._id == id, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with id {id}.");
                throw;
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            try
            {
                await _products.DeleteOneAsync(product => product._id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with id {id}.");
                throw;
            }
        }
    }
}
