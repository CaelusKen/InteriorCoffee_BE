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
        Task<(List<Voucher>, int, int, int, int)> GetVouchersAsync(int? pageNo, int? pageSize);
        Task<Voucher> GetVoucherById(string id);
        Task CreateVoucher(CreateVoucherDTO createVoucherDTO);
        Task UpdateVoucher(string id, UpdateVoucherDTO updatedVoucher);
        Task DeleteVoucher(string id);
    }
}
