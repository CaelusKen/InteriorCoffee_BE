using InteriorCoffee.Application.DTOs.VoucherType;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IVoucherTypeService
    {
        public Task<(List<VoucherType>, int, int, int, int)> GetVoucherTypesAsync(int? pageNo, int? pageSize);
        public Task<VoucherType> GetVoucherTypeById(string id);
        public Task CreateVoucherType(VoucherTypeDTO voucherTypeDTO);
        public Task UpdateVoucherType(string id, VoucherTypeDTO voucherTypeDTO);
        public Task DeleteVoucherType(string id);
    }
}
