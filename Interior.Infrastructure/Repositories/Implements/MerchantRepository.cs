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
    public class MerchantRepository : BaseRepository<MerchantRepository>, IMerchantRepository
    {
        private readonly IMongoCollection<Merchant> _merchants;
        private readonly ILogger<MerchantRepository> _logger;

        public MerchantRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<MerchantRepository> logger) : base(setting, client)
        {
            _merchants = _database.GetCollection<Merchant>("Merchant");
            _logger = logger;
        }

        public async Task<(List<Merchant>, int, int, int)> GetMerchantsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _merchants.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var merchants = await _merchants.Find(new BsonDocument())
                                                .Skip((pageNumber - 1) * pageSize)
                                                .Limit(pageSize)
                                                .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (merchants, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated merchants.");
                throw;
            }
        }


        public async Task<Merchant> GetMerchantById(string id)
        {
            return await _merchants.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        #region Get Function
        public async Task<Merchant> GetMerchant(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _merchants.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetMerchant<TResult>(Expression<Func<Merchant, TResult>> selector, Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _merchants.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Merchant>> GetMerchantList(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).ToListAsync();

            return await _merchants.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetMerchantList<TResult>(Expression<Func<Merchant, TResult>> selector, Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _merchants.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Merchant>> GetMerchantPagination(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _merchants.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetMerchantPagination<TResult>(Expression<Func<Merchant, TResult>> selector, Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Merchant>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _merchants.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _merchants.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateMerchant(Merchant merchant)
        {
            await _merchants.ReplaceOneAsync(a => a._id == merchant._id, merchant);
        }

        public async Task CreateMerchant(Merchant merchant)
        {
            await _merchants.InsertOneAsync(merchant);
        }

        public async Task DeleteMerchant(string id)
        {
            FilterDefinition<Merchant> filterDefinition = Builders<Merchant>.Filter.Eq("_id", id);
            await _merchants.DeleteOneAsync(filterDefinition);
        }
    }
}
