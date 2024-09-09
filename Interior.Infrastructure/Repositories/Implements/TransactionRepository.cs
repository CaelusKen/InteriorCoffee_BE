using Amazon.Runtime.Internal.Util;
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
    public class TransactionRepository : BaseRepository<TransactionRepository>, ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactions;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<TransactionRepository> logger) : base(setting, client)
        {
            _transactions = _database.GetCollection<Transaction>("Transaction");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<List<Transaction>> GetTransactionList(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _transactions.Find(filter).SortBy(orderBy).ToListAsync();

            return await _transactions.Find(filter).ToListAsync();
        }

        public async Task<Transaction> GetTransaction(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _transactions.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _transactions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateTransaction(Transaction Transaction)
        {
            await _transactions.ReplaceOneAsync(a => a._id == Transaction._id, Transaction);
        }

        public async Task CreateTransaction(Transaction Transaction)
        {
            await _transactions.InsertOneAsync(Transaction);
        }

        public async Task DeleteTransaction(string id)
        {
            FilterDefinition<Transaction> filterDefinition = Builders<Transaction>.Filter.Eq("_id", id);
            await _transactions.DeleteOneAsync(filterDefinition);
        }
        #endregion
    }
}
