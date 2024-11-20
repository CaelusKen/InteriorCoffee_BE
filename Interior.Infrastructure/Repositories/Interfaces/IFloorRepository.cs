using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interior.Infrastructure.Repositories.Interfaces
{
    public interface IFloorRepository
    {
        #region Get Functions
        Task<(List<Floor>, int)> GetFloorAsync();
        Task<Floor> GetFloorById(string id);
        Task<List<Floor>> GetFloorsByIdList(List<string> ids);

        #region Dynamic Get Function
        Task<Floor> GetFloor(Expression<Func<Floor, bool>> predicate = null,
                                 Expression<Func<Floor, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetFloor<TResult>(Expression<Func<Floor, TResult>> selector,
                                          Expression<Func<Floor, bool>> predicate = null,
                                          Expression<Func<Floor, object>> orderBy = null, bool isAscend = true);
        Task<List<Floor>> GetFloorList(Expression<Func<Floor, bool>> predicate = null,
                                           Expression<Func<Floor, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetFloorList<TResult>(Expression<Func<Floor, TResult>> selector,
                                                    Expression<Func<Floor, bool>> predicate = null,
                                                    Expression<Func<Floor, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<Floor>> GetFloorPagination(Expression<Func<Floor, bool>> predicate = null,
                                                      Expression<Func<Floor, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetFloorPagination<TResult>(Expression<Func<Floor, TResult>> selector,
                                                               Expression<Func<Floor, bool>> predicate = null,
                                                               Expression<Func<Floor, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
        #endregion

        Task AddRange(List<Floor> floors);
        Task CreateFloor(Floor account);
        Task UpdateFloor(Floor account);
        Task DeleteFloor(string id);
        Task DeleteFloorsByIds(List<string> ids);
        Task DeleteAllFloorsInDesign(string designTemplateId);
    }
}
