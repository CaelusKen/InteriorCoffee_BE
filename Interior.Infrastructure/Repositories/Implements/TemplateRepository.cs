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

        #region Conditional Get
        public async Task<List<Template>> GetTemplateListByCondition(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).ToListAsync();

            return await _templates.Find(filter).ToListAsync();
        }

        public async Task<Template> GetTemplateByCondition(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null)
        {
            var filterBuilder = Builders<Template>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _templates.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _templates.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Template>> GetTemplateList()
        {
            try
            {
                return await _templates.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting template list.");
                throw;
            }
        }

        public async Task<Template> GetTemplateById(string id)
        {
            try
            {
                return await _templates.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting template with id {id}.");
                throw;
            }
        }

        public async Task UpdateTemplate(Template template)
        {
            try
            {
                await _templates.ReplaceOneAsync(a => a._id == template._id, template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating template with id {template._id}.");
                throw;
            }
        }

        public async Task CreateTemplate(Template template)
        {
            try
            {
                await _templates.InsertOneAsync(template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an template.");
                throw;
            }
        }

        public async Task DeleteTemplate(string id)
        {
            try
            {
                FilterDefinition<Template> filterDefinition = Builders<Template>.Filter.Eq("_id", id);
                await _templates.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting template with id {id}.");
                throw;
            }
        }
    }
}
