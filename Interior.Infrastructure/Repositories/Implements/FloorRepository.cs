using Interior.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Implements;
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

namespace Interior.Infrastructure.Repositories.Implements
{
    public class FloorRepository : BaseRepository<FloorRepository>, IFloorRepository
    {
        private readonly IMongoCollection<Floor> _floors;
        private readonly ILogger<FloorRepository> _logger;

        public FloorRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<FloorRepository> logger) : base(setting, client)
        {
            _floors = _database.GetCollection<Floor>("Floor");
            _logger = logger;
        }

        #region Get Functions
        public async Task<(List<Floor>, int)> GetFloorAsync()
        {
            try
            {
                var totalItemsLong = await _floors.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var floors = await _floors.Find(floor => true).ToListAsync();
                return (floors, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting floors.");
                throw;
            }
        }

        public async Task<Floor> GetFloorById(string id)
        {
            return await _floors.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Floor>> GetFloorsByIdList(List<string> ids)
        {
            var filter = Builders<Floor>.Filter.In("_id", ids);
            return await _floors.Find(filter).ToListAsync();
        }

        #region Dynamic Get Function
        public async Task<Floor> GetFloor(Expression<Func<Floor, bool>> predicate = null, Expression<Func<Floor, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _floors.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetFloor<TResult>(Expression<Func<Floor, TResult>> selector, Expression<Func<Floor, bool>> predicate = null,
            Expression<Func<Floor, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _floors.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Floor>> GetFloorList(Expression<Func<Floor, bool>> predicate = null, Expression<Func<Floor, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _floors.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetFloorList<TResult>(Expression<Func<Floor, TResult>> selector, Expression<Func<Floor, bool>> predicate = null,
            Expression<Func<Floor, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _floors.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Floor>> GetFloorPagination(Expression<Func<Floor, bool>> predicate = null, Expression<Func<Floor, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _floors.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetFloorPagination<TResult>(Expression<Func<Floor, TResult>> selector, Expression<Func<Floor, bool>> predicate = null,
            Expression<Func<Floor, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Floor>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _floors.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _floors.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _floors.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _floors.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion
        #endregion

        public async Task AddRange(List<Floor> floors)
        {
            await _floors.InsertManyAsync(floors);
        }

        public async Task CreateFloor(Floor floor)
        {
            await _floors.InsertOneAsync(floor);
        }

        public async Task UpdateFloor(Floor floor)
        {
            var filter = Builders<Floor>.Filter.Eq(f => f._id, floor._id);
            var result = await _floors.ReplaceOneAsync(filter, floor);

            if (result.ModifiedCount == 0)
            {
                throw new Exception($"No document found with id: {floor._id} to update.");
            }
        }

        public async Task DeleteFloor(string id)
        {
            FilterDefinition<Floor> filterDefinition = Builders<Floor>.Filter.Eq("_id", id);
            await _floors.DeleteOneAsync(filterDefinition);
        }

        public async Task DeleteFloorsByIds(List<string> ids)
        {
            var filter = Builders<Floor>.Filter.In("_id", ids);
            await _floors.DeleteManyAsync(filter);
        }

        public async Task DeleteAllFloorsInDesign(string designTemplateId)
        {
            var filter = Builders<Floor>.Filter.Eq("DesignTemplateId", designTemplateId);
            await _floors.DeleteManyAsync(filter);
        }
    }
}
