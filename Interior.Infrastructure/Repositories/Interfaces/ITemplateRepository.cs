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
    public interface ITemplateRepository
    {
        Task CreateTemplate(Template template);
        Task UpdateTemplate(Template template);
        Task DeleteTemplate(string id);

        Task<(List<Template>, int)> GetTemplatesAsync();

        #region Get Function
        Task<Template> GetTemplate(Expression<Func<Template, bool>> predicate = null,
                                 Expression<Func<Template, object>> orderBy = null);
        Task<TResult> GetTemplate<TResult>(Expression<Func<Template, TResult>> selector,
                                          Expression<Func<Template, bool>> predicate = null,
                                          Expression<Func<Template, object>> orderBy = null);
        Task<List<Template>> GetTemplateList(Expression<Func<Template, bool>> predicate = null,
                                           Expression<Func<Template, object>> orderBy = null);
        Task<List<TResult>> GetTemplateList<TResult>(Expression<Func<Template, TResult>> selector,
                                                    Expression<Func<Template, bool>> predicate = null,
                                                    Expression<Func<Template, object>> orderBy = null);
        Task<IPaginate<Template>> GetTemplatePagination(Expression<Func<Template, bool>> predicate = null,
                                                      Expression<Func<Template, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetTemplatePagination<TResult>(Expression<Func<Template, TResult>> selector,
                                                               Expression<Func<Template, bool>> predicate = null,
                                                               Expression<Func<Template, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
