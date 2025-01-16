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
    public class StyleRepository : BaseRepository<StyleRepository>, IStyleRepository
    {
        private readonly IMongoCollection<Style> _styles;
        private readonly ILogger<StyleRepository> _logger;

        public StyleRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<StyleRepository> logger) : base(setting, client)
        {
            _styles = _database.GetCollection<Style>("Style");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<(List<Style>, int)> GetStylesAsync()
        {
            try
            {
                var totalItemsLong = await _styles.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var styles = await _styles.Find(new BsonDocument()).ToListAsync();
                return (styles, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting styles.");
                throw;
            }
        }

        #region Get Function
        public async Task<Style> GetStyle(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _styles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetStyle<TResult>(Expression<Func<Style, TResult>> selector, Expression<Func<Style, bool>> predicate = null,
            Expression<Func<Style, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _styles.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Style>> GetStyleList(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _styles.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetStyleList<TResult>(Expression<Func<Style, TResult>> selector, Expression<Func<Style, bool>> predicate = null,
            Expression<Func<Style, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _styles.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Style>> GetStylePagination(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _styles.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetStylePagination<TResult>(Expression<Func<Style, TResult>> selector, Expression<Func<Style, bool>> predicate = null,
            Expression<Func<Style, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _styles.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _styles.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _styles.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _styles.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateStyle(Style style)
        {
            await _styles.ReplaceOneAsync(a => a._id == style._id, style);
        }

        public async Task CreateStyle(Style style)
        {
            await _styles.InsertOneAsync(style);
        }

        public async Task DeleteStyle(string id)
        {
            FilterDefinition<Style> filterDefinition = Builders<Style>.Filter.Eq("_id", id);
            await _styles.DeleteOneAsync(filterDefinition);
        }
        #endregion
    }
}
