using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface ICampaignProductsRepository
    {
        #region Get Function
        Task<CampaignProducts> GetCampaignProducts(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetCampaignProducts<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true);
        Task<List<CampaignProducts>> GetCampaignProductsList(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetCampaignProductsList<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<CampaignProducts>> GetCampaignProductsPagination(Expression<Func<CampaignProducts, bool>> predicate = null, Expression<Func<CampaignProducts, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetCampaignProductsPagination<TResult>(Expression<Func<CampaignProducts, TResult>> selector, Expression<Func<CampaignProducts, bool>> predicate = null,
            Expression<Func<CampaignProducts, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10);
        #endregion

        Task UpdateCampaignProducts(CampaignProducts CampaignProducts);
        Task CreateCampaignProducts(CampaignProducts CampaignProducts);
        Task DeleteCampaignProducts(string productId);
        Task DeleteAllProductsInCampaign(string campaignId);
    }
}
