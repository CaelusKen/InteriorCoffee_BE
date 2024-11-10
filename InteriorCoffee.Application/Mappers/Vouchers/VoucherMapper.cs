using AutoMapper;
using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Application.Enums.Voucher;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Vouchers
{
    public class VoucherMapper : Profile
    {
        public VoucherMapper()
        {
            CreateMap<CreateVoucherDTO, Voucher>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(des => des.Status, src => src.MapFrom(src => VoucherStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UsedAccountIds, src => src.MapFrom(src => new List<string>()));
        }
    }
}
