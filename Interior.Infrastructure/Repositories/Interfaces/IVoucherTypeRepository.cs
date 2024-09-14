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
        Task CreateVoucherType(VoucherType voucherType);
        Task UpdateVoucherType(VoucherType voucherType);
        Task DeleteVoucherType(string id);

        public Task<(List<VoucherType>, int, int, int)> GetVoucherTypesAsync(int pageNumber, int pageSize);
        public Task<VoucherType> GetVoucherType(Expression<Func<VoucherType, bool>> predicate = null, Expression<Func<VoucherType, object>> orderBy = null);
    }
}
