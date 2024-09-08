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
    public class VoucherRepository : BaseRepository<VoucherRepository>, IVoucherRepository
    {
        private readonly IMongoCollection<Voucher> _vouchers;
        private readonly ILogger<VoucherRepository> _logger;

        public VoucherRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<VoucherRepository> logger) : base(setting, client)
        {
            _vouchers = _database.GetCollection<Voucher>("Voucher");
            _logger = logger;
        }

        #region Conditional Get
        public async Task<List<Voucher>> GetVoucherListByCondition(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).ToListAsync();

            return await _vouchers.Find(filter).ToListAsync();
        }

        public async Task<Voucher> GetVoucherByCondition(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _vouchers.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Voucher>> GetVoucherList()
        {
            try
            {
                return await _vouchers.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting voucher list.");
                throw;
            }
        }

        public async Task<Voucher> GetVoucherById(string id)
        {
            try
            {
                return await _vouchers.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting voucher with id {id}.");
                throw;
            }
        }

        public async Task UpdateVoucher(Voucher voucher)
        {
            try
            {
                await _vouchers.ReplaceOneAsync(a => a._id == voucher._id, voucher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating voucher with id {voucher._id}.");
                throw;
            }
        }

        public async Task CreateVoucher(Voucher voucher)
        {
            try
            {
                await _vouchers.InsertOneAsync(voucher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an voucher.");
                throw;
            }
        }

        public async Task DeleteVoucher(string id)
        {
            try
            {
                FilterDefinition<Voucher> filterDefinition = Builders<Voucher>.Filter.Eq("_id", id);
                await _vouchers.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting voucher with id {id}.");
                throw;
            }
        }
    }
}
