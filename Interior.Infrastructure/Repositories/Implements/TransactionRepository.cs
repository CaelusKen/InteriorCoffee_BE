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
        public async Task<(List<Transaction>, int, int, int)> GetTransactionsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _transactions.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var transactions = await _transactions.Find(new BsonDocument())
                                                      .Skip((pageNumber - 1) * pageSize)
                                                      .Limit(pageSize)
                                                      .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (transactions, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated transactions.");
                throw;
            }
        }

        #region Get Function
        public async Task<Transaction> GetTransaction(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _transactions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetTransaction<TResult>(Expression<Func<Transaction, TResult>> selector, Expression<Func<Transaction, bool>> predicate = null,
            Expression<Func<Transaction, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _transactions.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetTransactionList(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _transactions.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetTransactionList<TResult>(Expression<Func<Transaction, TResult>> selector, Expression<Func<Transaction, bool>> predicate = null,
            Expression<Func<Transaction, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _transactions.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Transaction>> GetTransactionPagination(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _transactions.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetTransactionPagination<TResult>(Expression<Func<Transaction, TResult>> selector, Expression<Func<Transaction, bool>> predicate = null,
            Expression<Func<Transaction, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Transaction>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _transactions.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _transactions.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _transactions.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _transactions.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

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
