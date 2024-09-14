using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class ProductCategoryRepository : BaseRepository<ProductCategoryRepository>, IProductCategoryRepository
    {
        private readonly IMongoCollection<ProductCategory> _productCategories;
        private readonly ILogger<ProductCategoryRepository> _logger;

        public ProductCategoryRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<ProductCategoryRepository> logger) : base(setting, client)
        {
            _productCategories = _database.GetCollection<ProductCategory>("ProductCategory");
            _logger = logger;
        }

        #region Conditional Get
        public async Task<List<ProductCategory>> GetProductCategoryListByCondition(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _productCategories.Find(filter).SortBy(orderBy).ToListAsync();

            return await _productCategories.Find(filter).ToListAsync();
        }

        public async Task<ProductCategory> GetProductCategoryByCondition(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _productCategories.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _productCategories.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<(List<ProductCategory>, int, int, int)> GetProductCategoriesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _productCategories.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var productCategories = await _productCategories.Find(new BsonDocument())
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Limit(pageSize)
                                                                .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (productCategories, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated product categories.");
                throw;
            }
        }


        public async Task<ProductCategory> GetProductCategoryById(string id)
        {
            return await _productCategories.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateProductCategory(ProductCategory productCategory)
        {
            await _productCategories.ReplaceOneAsync(a => a._id == productCategory._id, productCategory);
        }

        public async Task CreateProductCategory(ProductCategory productCategory)
        {
            await _productCategories.InsertOneAsync(productCategory);
        }

        public async Task DeleteProductCategory(string id)
        {
            FilterDefinition<ProductCategory> filterDefinition = Builders<ProductCategory>.Filter.Eq("_id", id);
            await _productCategories.DeleteOneAsync(filterDefinition);
        }
    }
}
