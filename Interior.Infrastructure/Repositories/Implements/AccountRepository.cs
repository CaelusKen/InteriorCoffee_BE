using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using InteriorCoffee.Domain.Paginate;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<AccountRepository> logger) : base(setting, client)
        {
            _accounts = _database.GetCollection<Account>("Account");
            _logger = logger;
        }

        public async Task<(List<Account>, int)> GetAccountAsync()
        {
            try
            {
                var totalItemsLong = await _accounts.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var accounts = await _accounts.Find(account => true).ToListAsync();
                return (accounts, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting accounts.");
                throw;
            }
        }



        public async Task<Account> GetAccountById(string id)
        {
            return await _accounts.Find(c => c._id == id).FirstOrDefaultAsync();
        }


        #region Get Function
        public async Task<Account> GetAccount(Expression<Func<Account, bool>> predicate = null, Expression<Func<Account, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _accounts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetAccount<TResult>(Expression<Func<Account, TResult>> selector, Expression<Func<Account, bool>> predicate = null,
            Expression<Func<Account, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _accounts.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetAccountList(Expression<Func<Account, bool>> predicate = null, Expression<Func<Account, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _accounts.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetAccountList<TResult>(Expression<Func<Account, TResult>> selector, Expression<Func<Account, bool>> predicate = null,
            Expression<Func<Account, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _accounts.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Account>> GetAccountPagination(Expression<Func<Account, bool>> predicate = null, Expression<Func<Account, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _accounts.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetAccountPagination<TResult>(Expression<Func<Account, TResult>> selector, Expression<Func<Account, bool>> predicate = null,
            Expression<Func<Account, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _accounts.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _accounts.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _accounts.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _accounts.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateAccount(Account account)
        {
            await _accounts.ReplaceOneAsync(a => a._id == account._id, account);
        }

        public async Task CreateAccount(Account account)
        {
            await _accounts.InsertOneAsync(account);
        }

        public async Task DeleteAccount(string id)
        {
            FilterDefinition<Account> filterDefinition = Builders<Account>.Filter.Eq("_id", id);
            await _accounts.DeleteOneAsync(filterDefinition);
        }
    }
}
