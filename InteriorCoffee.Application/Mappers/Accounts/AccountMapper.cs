using AutoMapper;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Accounts
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisteredDTO, Account>()
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            CreateMap<MerchantRegisteredDTO, Account>()
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}
