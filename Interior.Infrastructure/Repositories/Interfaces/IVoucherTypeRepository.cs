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
    public interface IVoucherTypeRepository
    {
        Task CreateVoucherType(VoucherType voucherType);
        Task UpdateVoucherType(VoucherType voucherType);
        Task DeleteVoucherType(string id);

        Task<(List<VoucherType>, int)> GetVoucherTypesAsync();

        #region Get Function
        Task<VoucherType> GetVoucherType(Expression<Func<VoucherType, bool>> predicate = null,
                                 Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetVoucherType<TResult>(Expression<Func<VoucherType, TResult>> selector,
                                          Expression<Func<VoucherType, bool>> predicate = null,
                                          Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true);
        Task<List<VoucherType>> GetVoucherTypeList(Expression<Func<VoucherType, bool>> predicate = null,
                                           Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetVoucherTypeList<TResult>(Expression<Func<VoucherType, TResult>> selector,
                                                    Expression<Func<VoucherType, bool>> predicate = null,
                                                    Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<VoucherType>> GetVoucherTypePagination(Expression<Func<VoucherType, bool>> predicate = null,
                                                      Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetVoucherTypePagination<TResult>(Expression<Func<VoucherType, TResult>> selector,
                                                               Expression<Func<VoucherType, bool>> predicate = null,
                                                               Expression<Func<VoucherType, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
