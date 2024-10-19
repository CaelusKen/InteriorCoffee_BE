using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class CampaignProductsRepository : BaseRepository<CampaignProductsRepository>, ICampaignProductsRepository
    {
        private readonly IMongoCollection<CampaignProducts> _products;

        public CampaignProductsRepository(IOptions<MongoDBContext> setting, IMongoClient client) : base(setting, client)
        {
            _products = _database.GetCollection<CampaignProducts>("CampaignProducts");
        }

        #region Get Function
        public async Task<CampaignProducts> GetCampaignProducts(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _products.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _products.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetCampaignProducts<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
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

        public async Task<List<CampaignProducts>> GetCampaignProductsList(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
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

        public async Task<List<TResult>> GetCampaignProductsList<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
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

        public async Task<IPaginate<CampaignProducts>> GetCampaignProductsPagination(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
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

        public async Task<IPaginate<TResult>> GetCampaignProductsPagination<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<CampaignProducts>.Filter;
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

        public async Task UpdateCampaignProducts(CampaignProducts CampaignProducts)
        {
            await _products.ReplaceOneAsync(cp => cp.ProductId== CampaignProducts.ProductId, CampaignProducts);
        }

        public async Task CreateCampaignProducts(CampaignProducts CampaignProducts)
        {
            await _products.InsertOneAsync(CampaignProducts);
        }

        public async Task DeleteCampaignProducts(string productId)
        {
            FilterDefinition<CampaignProducts> filterDefinition = Builders<CampaignProducts>.Filter.Eq("ProductId", productId);
            await _products.DeleteOneAsync(filterDefinition);
        }

        public async Task DeleteAllProductsInCampaign(string campaignId)
        {
            FilterDefinition<CampaignProducts> filterDefinition = Builders<CampaignProducts>.Filter.Eq("CampaignId", campaignId);
            await _products.DeleteManyAsync(filterDefinition);
        }
    }
}
