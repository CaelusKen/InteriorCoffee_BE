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

        #region Conditional Get
        public async Task<List<Transaction>> GetTransactionListByCondition(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _transactions.Find(filter).SortBy(orderBy).ToListAsync();

            return await _transactions.Find(filter).ToListAsync();
        }

        public async Task<Transaction> GetTransactionByCondition(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _transactions.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _transactions.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Transaction>> GetTransactionList()
        {
            try
            {
                return await _transactions.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting transaction list.");
                throw;
            }
        }

        public async Task<Transaction> GetTransactionById(string id)
        {
            try
            {
                return await _transactions.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting transaction with id {id}.");
                throw;
            }
        }

        public async Task UpdateTransaction(Transaction Transaction)
        {
            try
            {
                await _transactions.ReplaceOneAsync(a => a._id == Transaction._id, Transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating transaction with id {Transaction._id}.");
                throw;
            }
        }

        public async Task CreateTransaction(Transaction Transaction)
        {
            try
            {
                await _transactions.InsertOneAsync(Transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an transaction.");
                throw;
            }
        }

        public async Task DeleteTransaction(string id)
        {
            try
            {
                FilterDefinition<Transaction> filterDefinition = Builders<Transaction>.Filter.Eq("_id", id);
                await _transactions.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting transaction with id {id}.");
                throw;
            }
        }
    }
}
