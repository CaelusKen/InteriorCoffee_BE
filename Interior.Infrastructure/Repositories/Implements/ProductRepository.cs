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

        public async Task<(List<Product>, int)> GetProductsAsync()
        {
            try
            {
                var totalItemsLong = await _products.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var products = await _products.Find(product => true).ToListAsync();
                return (products, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products.");
                throw;
            }
        }


        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products.Find<Product>(product => product._id == id).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(prod => prod._id == id, product);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _products.DeleteOneAsync(product => product._id == id);
        }
    }
}
