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
        Task CreateStyle(Style style);
        Task UpdateStyle(Style style);
        Task DeleteStyle(string id);

        public Task<List<Style>> GetStyleList(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null);
        public Task<Style> GetStyle(Expression<Func<Style, bool>> predicate = null, Expression<Func<Style, object>> orderBy = null);
    }
}
