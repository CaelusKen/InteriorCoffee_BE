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
        Task<List<Role>> GetRoleList();
        Task<Role> GetRoleById(string id);
        Task CreateRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(string id);

        public Task<List<Role>> GetRoleListByCondition(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null);
        public Task<Role> GetRoleByCondition(Expression<Func<Role, bool>> predicate = null, Expression<Func<Role, object>> orderBy = null);
    }
}
