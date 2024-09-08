using AutoMapper;
using InteriorCoffee.Application.DTOs.VoucherType;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.VoucherTypes
{
    public class VoucherTypeMapper : Profile
    {
        public VoucherTypeMapper()
        {
            CreateMap<VoucherTypeDTO, VoucherType>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}
