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
    public class TemplateRepository : BaseRepository<TemplateRepository>, ITemplateRepository
    {
        private readonly IMongoCollection<Template> _templates;
        private readonly ILogger<TemplateRepository> _logger;

        public TemplateRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<TemplateRepository> logger) : base(setting, client)
        {
            _templates = _database.GetCollection<Template>("Template");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<(List<Template>, int, int, int)> GetTemplatesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _templates.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var templates = await _templates.Find(new BsonDocument())
                                                .Skip((pageNumber - 1) * pageSize)
                                                .Limit(pageSize)
                                                .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (templates, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated templates.");
                throw;
            }
        }

        #region Get Function
        public async Task<Template> GetTemplate(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _templates.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetTemplate<TResult>(Expression<Func<Template, TResult>> selector, Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _templates.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Template>> GetTemplateList(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).ToListAsync();

            return await _templates.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetTemplateList<TResult>(Expression<Func<Template, TResult>> selector, Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _templates.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Template>> GetTemplatePagination(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _templates.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetTemplatePagination<TResult>(Expression<Func<Template, TResult>> selector, Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _templates.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateTemplate(Template template)
        {
            await _templates.ReplaceOneAsync(a => a._id == template._id, template);
        }

        public async Task CreateTemplate(Template template)
        {
            await _templates.InsertOneAsync(template);
        }

        public async Task DeleteTemplate(string id)
        {
            FilterDefinition<Template> filterDefinition = Builders<Template>.Filter.Eq("_id", id);
            await _templates.DeleteOneAsync(filterDefinition);
        }
        #endregion
    }
}
