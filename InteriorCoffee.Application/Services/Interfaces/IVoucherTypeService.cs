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
        public Task<List<VoucherType>> GetAllVouchers();
        public Task<VoucherType> GetVoucherById(string id);
        public Task CreateVoucher(VoucherTypeDTO voucherTypeDTO);
        public Task UpdateVoucher(string id, VoucherTypeDTO voucherTypeDTO);
        public Task DeleteVoucher(string id);
    }
}
