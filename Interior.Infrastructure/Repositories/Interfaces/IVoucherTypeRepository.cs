using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IVoucherTypeRepository
    {
        Task<List<VoucherType>> GetVoucherTypeList();
        Task<VoucherType> GetVoucherTypeById(string id);
        Task CreateVoucherType(VoucherType voucherType);
        Task UpdateVoucherType(VoucherType voucherType);
        Task DeleteVoucherType(string id);

        public Task<List<VoucherType>> GetVoucherTypeListByCondition(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null);
        public Task<VoucherType> GetVoucherTypeByCondition(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null);
    }
}
