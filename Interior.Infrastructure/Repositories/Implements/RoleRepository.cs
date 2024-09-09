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
    public class RoleRepository : BaseRepository<RoleRepository>, IRoleRepository
    {
        private readonly IMongoCollection<Role> _roles;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<RoleRepository> logger) : base(setting, client)
        {
            _roles = _database.GetCollection<Role>("Role");
            _logger = logger;
        }
        
        #region CRUD Functions
        public async Task<List<Role>> GetRoleList(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _roles.Find(filter).SortBy(orderBy).ToListAsync();

            return await _roles.Find(filter).ToListAsync();
        }

        public async Task<Role> GetRole(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _roles.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _roles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateRole(Role role)
        {
            await _roles.ReplaceOneAsync(a => a._id == role._id, role);
        }

        public async Task CreateRole(Role role)
        {
            await _roles.InsertOneAsync(role);
        }

        public async Task DeleteRole(string id)
        {
            FilterDefinition<Role> filterDefinition = Builders<Role>.Filter.Eq("_id", id);
            await _roles.DeleteOneAsync(filterDefinition);
        }
        #endregion

    }
}
