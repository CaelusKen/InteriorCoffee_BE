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
    public class DesignRepository : BaseRepository<DesignRepository>, IDesignRepository
    {
        private readonly IMongoCollection<Design> _designs;
        private readonly ILogger<DesignRepository> _logger;

        public DesignRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<DesignRepository> logger) : base(setting, client)
        {
            _designs = _database.GetCollection<Design>("Design");
            _logger = logger;
        }

        #region Conditional Get
        public async Task<List<Design>> GetDesignListByCondition(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _designs.Find(filter).SortBy(orderBy).ToListAsync();

            return await _designs.Find(filter).ToListAsync();
        }

        public async Task<Design> GetDesignByCondition(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null)
        {
            var filterBuilder = Builders<Design>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _designs.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _designs.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<(List<Design>, int, int, int)> GetDesignsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _designs.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var designs = await _designs.Find(new BsonDocument())
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Limit(pageSize)
                                            .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (designs, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated designs.");
                throw;
            }
        }


        public async Task<Design> GetDesignById(string id)
        {
            return await _designs.Find(c => c._id == id).FirstOrDefaultAsync();
        }

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
