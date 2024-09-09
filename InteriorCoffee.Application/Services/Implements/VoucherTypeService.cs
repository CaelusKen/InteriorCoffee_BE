using AutoMapper;
using InteriorCoffee.Application.DTOs.Role;
using InteriorCoffee.Application.DTOs.VoucherType;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Implements;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class VoucherTypeService : BaseService<VoucherTypeService>, IVoucherTypeService
    {
        private readonly IVoucherTypeRepository _voucherTypeRepository;

        public VoucherTypeService(ILogger<VoucherTypeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IVoucherTypeRepository voucherTypeRepository) : base(logger, mapper, httpContextAccessor)
        {
            _voucherTypeRepository = voucherTypeRepository;
        }

        public async Task<List<VoucherType>> GetAllVoucherTypes()
        {
            return await _voucherTypeRepository.GetVoucherTypeList();
        }

        public async Task<VoucherType> GetVoucherTypeById(string id)
        {
            return await _voucherTypeRepository.GetVoucherType(
                predicate: t => t._id.Equals(id));
        }

        public async Task CreateVoucherType(VoucherTypeDTO voucherTypeDTO)
        {
            VoucherType newType = _mapper.Map<VoucherType>(voucherTypeDTO);
            await _voucherTypeRepository.CreateVoucherType(newType);
        }

        public async Task UpdateVoucherType(string id, VoucherTypeDTO voucherTypeDTO)
        {
            VoucherType type = await _voucherTypeRepository.GetVoucherType(
                predicate: t => t._id.Equals(id));

            if (type == null) throw new NotFoundException($"Voucher type id {id} cannot be found");

            //Update type data
            type.Name = String.IsNullOrEmpty(voucherTypeDTO.Name) ? type.Name : voucherTypeDTO.Name;
            type.Description = String.IsNullOrEmpty(voucherTypeDTO.Description) ? type.Description : voucherTypeDTO.Description;

            await _voucherTypeRepository.UpdateVoucherType(type);
        }

        public async Task DeleteVoucherType(string id)
        {
            VoucherType type = await _voucherTypeRepository.GetVoucherType(
                predicate: t => t._id.Equals(id));

            if (type == null) throw new NotFoundException($"Voucher type id {id} cannot be found");

            await _voucherTypeRepository.DeleteVoucherType(id);
        }
    }
}
