using InteriorCoffee.Domain.Models;
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
    public class DesignRepository : BaseRepository<DesignRepository>, IDesignRepository
    {
        private readonly IMongoCollection<Design> _designs;
        private readonly ILogger<DesignRepository> _logger;

        public DesignRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<DesignRepository> logger) : base(setting, client)
        {
            _designs = _database.GetCollection<Design>("Design");
            _logger = logger;
        }

        public async Task<(List<Design>, int)> GetDesignsAsync()
        {
            try
            {
                var totalItemsLong = await _designs.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var designs = await _designs.Find(new BsonDocument()).ToListAsync();
                return (designs, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting designs.");
                throw;
            }
        }

        public async Task<Design> GetDesignById(string id)
        {
            return await _designs.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        #region Get Function
        public async Task<Design> GetDesign(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _designs.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetDesign<TResult>(Expression<Func<Design, TResult>> selector, Expression<Func<Design, bool>> predicate = null,
            Expression<Func<Design, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _designs.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Design>> GetDesignList(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _designs.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetDesignList<TResult>(Expression<Func<Design, TResult>> selector, Expression<Func<Design, bool>> predicate = null,
            Expression<Func<Design, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _designs.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Design>> GetDesignPagination(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _designs.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetDesignPagination<TResult>(Expression<Func<Design, TResult>> selector, Expression<Func<Design, bool>> predicate = null,
            Expression<Func<Design, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _designs.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _designs.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _designs.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _designs.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateDesign(Design design)
        {
            await _designs.ReplaceOneAsync(a => a._id == design._id, design);
        }

        public async Task CreateDesign(Design design)
        {
            await _designs.InsertOneAsync(design);
        }

        public async Task DeleteDesign(string id)
        {
            FilterDefinition<Design> filterDefinition = Builders<Design>.Filter.Eq("_id", id);
            await _designs.DeleteOneAsync(filterDefinition);
        }
    }
}
