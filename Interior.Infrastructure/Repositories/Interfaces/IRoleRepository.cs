using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetRoleList(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null);
        public Task<Role> GetRole(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null);
        Task CreateRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(string id);
    }
}
