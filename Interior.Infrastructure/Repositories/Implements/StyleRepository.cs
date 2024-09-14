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
        public async Task<List<Style>> GetStyleList(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _styles.Find(filter).SortBy(orderBy).ToListAsync();

            return await _styles.Find(filter).ToListAsync();
        }

        public async Task<Style> GetStyle(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null)
        {
            var filterBuilder = Builders<Style>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _styles.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _styles.Find(filter).FirstOrDefaultAsync();
        }

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
