using AutoMapper;
using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class VoucherService : BaseService<VoucherService>, IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(ILogger<VoucherService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IVoucherRepository voucherRepository) : base(logger, mapper, httpContextAccessor)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<List<Voucher>> GetAllVouchers()
        {
            return await _voucherRepository.GetVoucherListByCondition();
        }

        public async Task<Voucher> GetVoucherById(string id)
        {
            return await _voucherRepository.GetVoucherByCondition(
                predicate: v => v._id.Equals(id));
        }

        public async Task CreateVoucher(CreateVoucherDTO createVoucherDTO)
        {
            Voucher newVoucher = _mapper.Map<Voucher>(createVoucherDTO);
            await _voucherRepository.CreateVoucher(newVoucher);
        }

        public async Task UpdateVoucher(string id, UpdateVoucherDTO updatedVoucher)
        {
            Voucher voucher = await _voucherRepository.GetVoucherByCondition(
                predicate: v => v._id.Equals(id));

            if (voucher == null) throw new NotFoundException($"Voucher id {id} cannot be found");

            //Update voucher data
            voucher.Code = String.IsNullOrEmpty(updatedVoucher.Code) ? voucher.Code : updatedVoucher.Code;
            voucher.Name = String.IsNullOrEmpty(updatedVoucher.Name) ? voucher.Name : updatedVoucher.Name;
            voucher.Description = String.IsNullOrEmpty(updatedVoucher.Description) ? voucher.Description : updatedVoucher.Description;
            voucher.DiscountPercentage = updatedVoucher.DiscountPercentage;
            voucher.Status = String.IsNullOrEmpty(updatedVoucher.Status) ? voucher.Status : updatedVoucher.Status;
            voucher.StartDate = updatedVoucher.StartDate;
            voucher.EndDate = updatedVoucher.EndDate;
            voucher.UpdatedDate = DateTime.Now;
            voucher.MaxUse = updatedVoucher.MaxUse;
            voucher.MinOrderValue = updatedVoucher.MinOrderValue;
            //voucher.UsedAccountIds = new List<string>();
            voucher.TypeId = String.IsNullOrEmpty(updatedVoucher.TypeId) ? voucher.TypeId : updatedVoucher.TypeId;

            await _voucherRepository.UpdateVoucher(voucher);
        }

        public async Task DeleteVoucher(string id)
        {
            Voucher voucher = await _voucherRepository.GetVoucherByCondition(
                predicate: v => v._id.Equals(id));

            if (voucher == null) throw new NotFoundException($"Voucher id {id} cannot be found");

            await _voucherRepository.DeleteVoucher(id);
        }
    }
}
