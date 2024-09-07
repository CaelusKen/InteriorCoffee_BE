using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class RoleRepository : BaseRepository<RoleRepository>, IRoleRepository
    {
        private readonly IMongoCollection<Role> _roles;
        public RoleRepository(IOptions<MongoDBContext> setting, IMongoClient client) : base(setting, client)
        {
            _roles = _database.GetCollection<Role>("Role");
        }

        public async Task<Role> GetRoleById(string id)
        {
            return await _roles.Find(r => r._id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _roles.Find(r => r.Name.Equals(roleName)).FirstOrDefaultAsync();
        }
    }
}
