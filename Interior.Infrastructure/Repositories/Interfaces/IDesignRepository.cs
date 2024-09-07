using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IDesignRepository
    {
        Task<List<Design>> GetDesignList();
        Task<Design> GetDesignById(string id);
        Task CreateDesign(Design design);
        Task UpdateDesign(Design design);
        Task DeleteDesign(string id);

        public Task<List<Design>> GetDesignListByCondition(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null);
        public Task<Design> GetDesignByCondition(Expression<Func<Design, bool>> predicate = null, Expression<Func<Design, object>> orderBy = null);
    }
}
