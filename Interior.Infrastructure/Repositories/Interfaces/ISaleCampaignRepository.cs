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
    public interface ISaleCampaignRepository
    {
        Task<(List<SaleCampaign>, int, int, int)> GetSaleCampaignsAsync(int pageNumber, int pageSize);
        Task<SaleCampaign> GetSaleCampaignById(string id);
        Task CreateSaleCampaign(SaleCampaign saleCampaign);
        Task UpdateSaleCampaign(SaleCampaign saleCampaign);
        Task DeleteSaleCampaign(string id);

        #region Get Function
        Task<SaleCampaign> GetSaleCampaign(Expression<Func<SaleCampaign, bool>> predicate = null,
                                 Expression<Func<SaleCampaign, object>> orderBy = null);
        Task<TResult> GetSaleCampaign<TResult>(Expression<Func<SaleCampaign, TResult>> selector,
                                          Expression<Func<SaleCampaign, bool>> predicate = null,
                                          Expression<Func<SaleCampaign, object>> orderBy = null);
        Task<List<SaleCampaign>> GetSaleCampaignList(Expression<Func<SaleCampaign, bool>> predicate = null,
                                           Expression<Func<SaleCampaign, object>> orderBy = null);
        Task<List<TResult>> GetSaleCampaignList<TResult>(Expression<Func<SaleCampaign, TResult>> selector,
                                                    Expression<Func<SaleCampaign, bool>> predicate = null,
                                                    Expression<Func<SaleCampaign, object>> orderBy = null);
        Task<IPaginate<SaleCampaign>> GetSaleCampaignPagination(Expression<Func<SaleCampaign, bool>> predicate = null,
                                                      Expression<Func<SaleCampaign, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetSaleCampaignPagination<TResult>(Expression<Func<SaleCampaign, TResult>> selector,
                                                               Expression<Func<SaleCampaign, bool>> predicate = null,
                                                               Expression<Func<SaleCampaign, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
