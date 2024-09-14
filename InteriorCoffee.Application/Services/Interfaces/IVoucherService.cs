using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IVoucherService
    {
        public Task<(List<Voucher>, int, int, int, int)> GetVouchersAsync(int? pageNo, int? pageSize);
        public Task<Voucher> GetVoucherById(string id);
        public Task CreateVoucher(CreateVoucherDTO createVoucherDTO);
        public Task UpdateVoucher(string id, UpdateVoucherDTO updatedVoucher);
        public Task DeleteVoucher(string id);
    }
}
