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
    public class MerchantRepository : BaseRepository<MerchantRepository>, IMerchantRepository
    {
        private readonly IMongoCollection<Merchant> _merchants;
        private readonly ILogger<MerchantRepository> _logger;

        public MerchantRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<MerchantRepository> logger) : base(setting, client)
        {
            _merchants = _database.GetCollection<Merchant>("Merchant");
            _logger = logger;
        }

        #region Conditional Get
        public async Task<List<Merchant>> GetMerchantListByCondition(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).ToListAsync();

            return await _merchants.Find(filter).ToListAsync();
        }

        public async Task<Merchant> GetMerchantByCondition(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _merchants.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Merchant>> GetMerchantList()
        {
            try
            {
                return await _merchants.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting merchant list.");
                throw;
            }
        }

        public async Task<Merchant> GetMerchantById(string id)
        {
            try
            {
                return await _merchants.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting merchant with id {id}.");
                throw;
            }
        }

        public async Task UpdateMerchant(Merchant merchant)
        {
            try
            {
                await _merchants.ReplaceOneAsync(a => a._id == merchant._id, merchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating merchant with id {merchant._id}.");
                throw;
            }
        }

        public async Task CreateMerchant(Merchant merchant)
        {
            try
            {
                await _merchants.InsertOneAsync(merchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an merchant.");
                throw;
            }
        }

        public async Task DeleteMerchant(string id)
        {
            try
            {
                FilterDefinition<Merchant> filterDefinition = Builders<Merchant>.Filter.Eq("_id", id);
                await _merchants.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting merchant with id {id}.");
                throw;
            }
        }
    }
}
