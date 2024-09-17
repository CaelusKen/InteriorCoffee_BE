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

        public async Task<Voucher> GetVoucher(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null)
        {
            var filterBuilder = Builders<Voucher>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _vouchers.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _vouchers.Find(filter).FirstOrDefaultAsync();
        }

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
