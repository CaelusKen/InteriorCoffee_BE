using InteriorCoffee.Domain.Models;
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
        Task<List<Merchant>> GetMerchantList();
        Task<Merchant> GetMerchantById(string id);
        Task CreateMerchant(Merchant merchant);
        Task UpdateMerchant(Merchant merchant);
        Task DeleteMerchant(string id);

        public Task<List<Merchant>> GetMerchantListByCondition(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null);
        public Task<Merchant> GetMerchantByCondition(Expression<Func<Merchant, bool>> predicate = null, Expression<Func<Merchant, object>> orderBy = null);
    }
}
