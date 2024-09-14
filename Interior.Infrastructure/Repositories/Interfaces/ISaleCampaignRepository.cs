using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface ISaleCampaignRepository
    {
        Task<(List<SaleCampaign>, int, int, int)> GetSaleCampaignsAsync(int pageNumber, int pageSize);
        Task<SaleCampaign> GetSaleCampaignById(string id);
        Task CreateSaleCampaign(SaleCampaign saleCampaign);
        Task UpdateSaleCampaign(SaleCampaign saleCampaign);
        Task DeleteSaleCampaign(string id);

        public Task<List<SaleCampaign>> GetSaleCampaignListByCondition(Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null);
        public Task<SaleCampaign> GetSaleCampaignByCondition(Expression<Func<SaleCampaign, bool>> predicate = null, Expression<Func<SaleCampaign, object>> orderBy = null);
    }
}
