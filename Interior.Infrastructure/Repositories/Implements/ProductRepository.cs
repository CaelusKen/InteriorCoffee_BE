using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Linq.Expressions;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Options;
using Amazon.Runtime.Internal.Util;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<ProductRepository> logger) : base(setting, client)
        {
            _products = _database.GetCollection<Product>("Product");
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

        #region Get Function
        public async Task<Product> GetProduct(Expression<Func<Product, bool>> predicate = null, Expression<Func<Product, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if(isAscend) 
                    return await _products.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else 
                    return await _products.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetProduct<TResult>(Expression<Func<Product, TResult>> selector, Expression<Func<Product, bool>> predicate = null,
            Expression<Func<Product, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _products.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _products.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductList(Expression<Func<Product, bool>> predicate = null, Expression<Func<Product, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _products.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _products.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetProductList<TResult>(Expression<Func<Product, TResult>> selector, Expression<Func<Product, bool>> predicate = null,
            Expression<Func<Product, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _products.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _products.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Product>> GetProductPagination(Expression<Func<Product, bool>> predicate = null, Expression<Func<Product, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _products.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _products.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetProductPagination<TResult>(Expression<Func<Product, TResult>> selector, Expression<Func<Product, bool>> predicate = null,
            Expression<Func<Product, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _products.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _products.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _products.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

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
