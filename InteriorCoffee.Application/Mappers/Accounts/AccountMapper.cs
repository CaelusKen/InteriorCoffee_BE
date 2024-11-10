using AutoMapper;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Accounts
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisteredDTO, Account>()
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            CreateMap<MerchantRegisteredDTO, Account>()
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.UNVERIFIED.ToString()))
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            CreateMap<CreateAccountDTO, Account>()
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            CreateMap<UpdateAccountDTO, Account>()
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow));

            CreateMap<Account, AccountResponseItemDTO>();

            // Add mapping for soft delete to update UpdatedDate
            CreateMap<SoftDeleteResponseDto, Account>()
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, src => src.MapFrom(src => AccountStatusEnum.INACTIVE.ToString()));

            // Add mapping for create new account with CONSULTANT enum role 
        }
    }
}
