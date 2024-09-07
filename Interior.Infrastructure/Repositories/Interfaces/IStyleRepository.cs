using InteriorCoffee.Domain.Models;
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
        Task<List<Style>> GetStyleList();
        Task<Style> GetStyleById(string id);
        Task CreateStyle(Style style);
        Task UpdateStyle(Style style);
        Task DeleteStyle(string id);

        public Task<List<Style>> GetStyleListByCondition(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null);
        public Task<Style> GetStyleByCondition(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null);
    }
}
