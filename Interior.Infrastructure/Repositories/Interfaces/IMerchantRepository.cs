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
    public interface IMerchantRepository
    {
        Task<(List<Merchant>, int)> GetMerchantsAsync();
        Task<Merchant> GetMerchantById(string id);
        Task CreateMerchant(Merchant merchant);
        Task UpdateMerchant(Merchant merchant);
        Task DeleteMerchant(string id);

        #region Get Function
        Task<Merchant> GetMerchant(Expression<Func<Merchant, bool>> predicate = null,
                                 Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetMerchant<TResult>(Expression<Func<Merchant, TResult>> selector,
                                          Expression<Func<Merchant, bool>> predicate = null,
                                          Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true);
        Task<List<Merchant>> GetMerchantList(Expression<Func<Merchant, bool>> predicate = null,
                                           Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetMerchantList<TResult>(Expression<Func<Merchant, TResult>> selector,
                                                    Expression<Func<Merchant, bool>> predicate = null,
                                                    Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<Merchant>> GetMerchantPagination(Expression<Func<Merchant, bool>> predicate = null,
                                                      Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetMerchantPagination<TResult>(Expression<Func<Merchant, TResult>> selector,
                                                               Expression<Func<Merchant, bool>> predicate = null,
                                                               Expression<Func<Merchant, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
