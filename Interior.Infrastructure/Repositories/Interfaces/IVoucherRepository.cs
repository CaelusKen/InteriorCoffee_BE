using InteriorCoffee.Domain.Models;
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

        public Task<(List<Voucher>, int, int, int)> GetVouchersAsync(int pageNumber, int pageSize);
        public Task<Voucher> GetVoucher(Expression<Func<Voucher, bool>> predicate = null, Expression<Func<Voucher, object>> orderBy = null);
    }
}
