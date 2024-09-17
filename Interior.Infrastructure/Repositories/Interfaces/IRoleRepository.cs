using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
        Task CreateRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(string id);

        #region Get Function
        Task<Role> GetRole(Expression<Func<Role, bool>> predicate = null,
                                 Expression<Func<Role, object>> orderBy = null);
        Task<TResult> GetRole<TResult>(Expression<Func<Role, TResult>> selector,
                                          Expression<Func<Role, bool>> predicate = null,
                                          Expression<Func<Role, object>> orderBy = null);
        Task<List<Role>> GetRoleList(Expression<Func<Role, bool>> predicate = null,
                                           Expression<Func<Role, object>> orderBy = null);
        Task<List<TResult>> GetRoleList<TResult>(Expression<Func<Role, TResult>> selector,
                                                    Expression<Func<Role, bool>> predicate = null,
                                                    Expression<Func<Role, object>> orderBy = null);
        Task<IPaginate<Role>> GetRolePagination(Expression<Func<Role, bool>> predicate = null,
                                                      Expression<Func<Role, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetRolePagination<TResult>(Expression<Func<Role, TResult>> selector,
                                                               Expression<Func<Role, bool>> predicate = null,
                                                               Expression<Func<Role, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
