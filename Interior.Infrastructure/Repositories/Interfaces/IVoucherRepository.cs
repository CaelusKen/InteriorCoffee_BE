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
    public interface IVoucherRepository
    {
        Task CreateVoucher(Voucher voucher);
        Task UpdateVoucher(Voucher voucher);
        Task DeleteVoucher(string id);

        Task<(List<Voucher>, int)> GetVouchersAsync();

        #region Get Function
        Task<Voucher> GetVoucher(Expression<Func<Voucher, bool>> predicate = null,
                                 Expression<Func<Voucher, object>> orderBy = null);
        Task<TResult> GetVoucher<TResult>(Expression<Func<Voucher, TResult>> selector,
                                          Expression<Func<Voucher, bool>> predicate = null,
                                          Expression<Func<Voucher, object>> orderBy = null);
        Task<List<Voucher>> GetVoucherList(Expression<Func<Voucher, bool>> predicate = null,
                                           Expression<Func<Voucher, object>> orderBy = null);
        Task<List<TResult>> GetVoucherList<TResult>(Expression<Func<Voucher, TResult>> selector,
                                                    Expression<Func<Voucher, bool>> predicate = null,
                                                    Expression<Func<Voucher, object>> orderBy = null);
        Task<IPaginate<Voucher>> GetVoucherPagination(Expression<Func<Voucher, bool>> predicate = null,
                                                      Expression<Func<Voucher, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetVoucherPagination<TResult>(Expression<Func<Voucher, TResult>> selector,
                                                               Expression<Func<Voucher, bool>> predicate = null,
                                                               Expression<Func<Voucher, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
