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
using System.Transactions;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class SaleCampaignRepository : BaseRepository<SaleCampaignRepository>, ISaleCampaignRepository
    {
        private readonly IMongoCollection<SaleCampaign> _saleCampaigns;
        private readonly ILogger<SaleCampaignRepository> _logger;

        public SaleCampaignRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<SaleCampaignRepository> logger) : base(setting, client)
        {
            _saleCampaigns = _database.GetCollection<SaleCampaign>("SaleCampaign");
            _logger = logger;
        }

        #region Get Function
        public async Task<SaleCampaign> GetSaleCampaign(Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _saleCampaigns.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetSaleCampaign<TResult>(Expression<Func<SaleCampaign, TResult>> selector, Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _saleCampaigns.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<SaleCampaign>> GetSaleCampaignList(Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).ToListAsync();

            return await _saleCampaigns.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetSaleCampaignList<TResult>(Expression<Func<SaleCampaign, TResult>> selector, Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _saleCampaigns.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<SaleCampaign>> GetSaleCampaignPagination(Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _saleCampaigns.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetSaleCampaignPagination<TResult>(Expression<Func<SaleCampaign, TResult>> selector, Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<SaleCampaign>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _saleCampaigns.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _saleCampaigns.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task<(List<SaleCampaign>, int, int, int)> GetSaleCampaignsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _saleCampaigns.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var saleCampaigns = await _saleCampaigns.Find(new BsonDocument())
                                                        .Skip((pageNumber - 1) * pageSize)
                                                        .Limit(pageSize)
                                                        .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (saleCampaigns, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated sale campaigns.");
                throw;
            }
        }


        public async Task<SaleCampaign> GetSaleCampaignById(string id)
        {
            try
            {
                return await _saleCampaigns.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting sale campaign with id {id}.");
                throw;
            }
        }

        public async Task UpdateSaleCampaign(SaleCampaign saleCampaign)
        {
            await _saleCampaigns.ReplaceOneAsync(a => a._id == saleCampaign._id, saleCampaign);
        }

        public async Task CreateSaleCampaign(SaleCampaign saleCampaign)
        {
            await _saleCampaigns.InsertOneAsync(saleCampaign);
        }

        public async Task DeleteSaleCampaign(string id)
        {
            FilterDefinition<SaleCampaign> filterDefinition = Builders<SaleCampaign>.Filter.Eq("_id", id);
            await _saleCampaigns.DeleteOneAsync(filterDefinition);
        }
    }
}
