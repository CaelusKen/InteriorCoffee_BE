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

        public async Task<List<Design>> GetDesignList()
        {
            try
            {
                return await _designs.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting design list.");
                throw;
            }
        }

        public async Task<Design> GetDesignById(string id)
        {
            try
            {
                return await _designs.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting design with id {id}.");
                throw;
            }
        }

        public async Task UpdateDesign(Design design)
        {
            try
            {
                await _designs.ReplaceOneAsync(a => a._id == design._id, design);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating design with id {design._id}.");
                throw;
            }
        }

        public async Task CreateDesign(Design design)
        {
            try
            {
                await _designs.InsertOneAsync(design);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an design.");
                throw;
            }
        }

        public async Task DeleteDesign(string id)
        {
            try
            {
                FilterDefinition<Design> filterDefinition = Builders<Design>.Filter.Eq("_id", id);
                await _designs.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting design with id {id}.");
                throw;
            }
        }
    }
}
