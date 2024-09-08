using InteriorCoffee.Domain.Models;
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
        Task<List<Template>> GetTemplateList();
        Task<Template> GetTemplateById(string id);
        Task CreateTemplate(Template template);
        Task UpdateTemplate(Template template);
        Task DeleteTemplate(string id);

        public Task<List<Template>> GetTemplateListByCondition(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null);
        public Task<Template> GetTemplateByCondition(Expression<Func<Template, bool>> predicate = null, Expression<Func<Template, object>> orderBy = null);
    }
}
