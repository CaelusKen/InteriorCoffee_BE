using Amazon.Runtime.Internal.Util;
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
    public class VoucherTypeRepository : BaseRepository<VoucherTypeRepository>, IVoucherTypeRepository
    {
        private readonly IMongoCollection<VoucherType> _voucherTypes;
        private readonly ILogger<VoucherTypeRepository> _logger;

        public VoucherTypeRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<VoucherTypeRepository> logger) : base(setting, client)
        {
            _voucherTypes = _database.GetCollection<VoucherType>("VoucherType");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<(List<VoucherType>, int, int, int)> GetVoucherTypesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _voucherTypes.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var voucherTypes = await _voucherTypes.Find(new BsonDocument())
                                                      .Skip((pageNumber - 1) * pageSize)
                                                      .Limit(pageSize)
                                                      .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (voucherTypes, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated voucher types.");
                throw;
            }
        }

        #region Get Function
        public async Task<VoucherType> GetVoucherType(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _voucherTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetVoucherType<TResult>(Expression<Func<VoucherType, TResult>> selector, Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _voucherTypes.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<VoucherType>> GetVoucherTypeList(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).ToListAsync();

            return await _voucherTypes.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetVoucherTypeList<TResult>(Expression<Func<VoucherType, TResult>> selector, Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _voucherTypes.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<VoucherType>> GetVoucherTypePagination(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _voucherTypes.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetVoucherTypePagination<TResult>(Expression<Func<VoucherType, TResult>> selector, Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<VoucherType>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _voucherTypes.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _voucherTypes.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateVoucherType(VoucherType voucherType)
        {
            await _voucherTypes.ReplaceOneAsync(a => a._id == voucherType._id, voucherType);
        }

        public async Task CreateVoucherType(VoucherType voucherType)
        {
            await _voucherTypes.InsertOneAsync(voucherType);
        }

        public async Task DeleteVoucherType(string id)
        {
            FilterDefinition<VoucherType> filterDefinition = Builders<VoucherType>.Filter.Eq("_id", id);
            await _voucherTypes.DeleteOneAsync(filterDefinition);
        }
        #endregion
    }
}
