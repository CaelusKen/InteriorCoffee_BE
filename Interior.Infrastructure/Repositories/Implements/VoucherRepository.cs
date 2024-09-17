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
    public class VoucherRepository : BaseRepository<VoucherRepository>, IVoucherRepository
    {
        private readonly IMongoCollection<Voucher> _vouchers;
        private readonly ILogger<VoucherRepository> _logger;

        public VoucherRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<VoucherRepository> logger) : base(setting, client)
        {
            _vouchers = _database.GetCollection<Voucher>("Voucher");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<(List<Voucher>, int)> GetVouchersAsync()
        {
            try
            {
                var totalItemsLong = await _vouchers.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var vouchers = await _vouchers.Find(new BsonDocument()).ToListAsync();
                return (vouchers, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vouchers.");
                throw;
            }
        }

        #region Get Function
        public async Task<Voucher> GetVoucher(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _vouchers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetVoucher<TResult>(Expression<Func<Voucher, TResult>> selector, Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _vouchers.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Voucher>> GetVoucherList(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).ToListAsync();

            return await _vouchers.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetVoucherList<TResult>(Expression<Func<Voucher, TResult>> selector, Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _vouchers.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Voucher>> GetVoucherPagination(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _vouchers.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetVoucherPagination<TResult>(Expression<Func<Voucher, TResult>> selector, Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _vouchers.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateVoucher(Voucher voucher)
        {
            await _vouchers.ReplaceOneAsync(a => a._id == voucher._id, voucher);
        }

        public async Task CreateVoucher(Voucher voucher)
        {
            await _vouchers.InsertOneAsync(voucher);
        }

        public async Task DeleteVoucher(string id)
        {
            FilterDefinition<Voucher> filterDefinition = Builders<Voucher>.Filter.Eq("_id", id);
            await _vouchers.DeleteOneAsync(filterDefinition);
        }
        #endregion
    }
}
