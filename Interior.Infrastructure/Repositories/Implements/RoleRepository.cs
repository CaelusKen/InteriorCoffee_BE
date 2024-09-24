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

        #region Get Function
        public async Task<Role> GetRole(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _roles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetRole<TResult>(Expression<Func<Role, TResult>> selector, Expression<Func<Role, bool>> predicate = null,
            Expression<Func<Role, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _roles.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Role>> GetRoleList(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _roles.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetRoleList<TResult>(Expression<Func<Role, TResult>> selector, Expression<Func<Role, bool>> predicate = null,
            Expression<Func<Role, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _roles.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Role>> GetRolePagination(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _roles.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetRolePagination<TResult>(Expression<Func<Role, TResult>> selector, Expression<Func<Role, bool>> predicate = null,
            Expression<Func<Role, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Role>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _roles.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _roles.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _roles.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _roles.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task<Role> GetRoleById(string id)
        {
            var filter = Builders<Role>.Filter.Eq("_id", id);
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
