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
    public interface IStyleRepository
    {
        Task CreateStyle(Style style);
        Task UpdateStyle(Style style);
        Task DeleteStyle(string id);

        Task<(List<Style>, int)> GetStylesAsync();

        #region Get Function
        Task<Style> GetStyle(Expression<Func<Style, bool>> predicate = null,
                                 Expression<Func<Style, object>> orderBy = null);
        Task<TResult> GetStyle<TResult>(Expression<Func<Style, TResult>> selector,
                                          Expression<Func<Style, bool>> predicate = null,
                                          Expression<Func<Style, object>> orderBy = null);
        Task<List<Style>> GetStyleList(Expression<Func<Style, bool>> predicate = null,
                                           Expression<Func<Style, object>> orderBy = null);
        Task<List<TResult>> GetStyleList<TResult>(Expression<Func<Style, TResult>> selector,
                                                    Expression<Func<Style, bool>> predicate = null,
                                                    Expression<Func<Style, object>> orderBy = null);
        Task<IPaginate<Style>> GetStylePagination(Expression<Func<Style, bool>> predicate = null,
                                                      Expression<Func<Style, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetStylePagination<TResult>(Expression<Func<Style, TResult>> selector,
                                                               Expression<Func<Style, bool>> predicate = null,
                                                               Expression<Func<Style, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
