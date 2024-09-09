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
        public async Task<List<Template>> GetTemplateList(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).ToListAsync();

            return await _templates.Find(filter).ToListAsync();
        }

        public async Task<Template> GetTemplate(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _templates.Find(filter).FirstOrDefaultAsync();
        }

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
