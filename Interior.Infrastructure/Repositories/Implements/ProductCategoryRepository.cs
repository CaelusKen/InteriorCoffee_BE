﻿using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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

        public async Task<(List<ProductCategory>, int)> GetProductCategoriesAsync()
        {
            try
            {
                var totalItemsLong = await _productCategories.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var productCategories = await _productCategories.Find(new BsonDocument()).ToListAsync();
                return (productCategories, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product categories.");
                throw;
            }
        }

        public async Task<bool> CategoryExistsAsync(string categoryId)
        {
            var (categories, _) = await GetProductCategoriesAsync();
            return categories.Any(c => c._id == categoryId);
        }




        public async Task<ProductCategory> GetProductCategoryById(string id)
        {
            return await _productCategories.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        #region Get Function
        public async Task<ProductCategory> GetProductCategory(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _productCategories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetProductCategory<TResult>(Expression<Func<ProductCategory, TResult>> selector, Expression<Func<ProductCategory, bool>> predicate = null,
            Expression<Func<ProductCategory, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _productCategories.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoryList(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _productCategories.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetProductCategoryList<TResult>(Expression<Func<ProductCategory, TResult>> selector, Expression<Func<ProductCategory, bool>> predicate = null,
            Expression<Func<ProductCategory, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _productCategories.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<ProductCategory>> GetProductCategoryPagination(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _productCategories.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetProductCategoryPagination<TResult>(Expression<Func<ProductCategory, TResult>> selector, Expression<Func<ProductCategory, bool>> predicate = null,
            Expression<Func<ProductCategory, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<ProductCategory>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _productCategories.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _productCategories.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _productCategories.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _productCategories.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

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
