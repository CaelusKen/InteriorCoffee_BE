using AutoMapper;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.DTOs.Merchant;
using InteriorCoffee.Application.Enums.Merchant;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Merchants
{
    public class MerchantMapper : Profile
    {
        public MerchantMapper()
        {
            // Mapping for CreateMerchantDTO to Merchant
            CreateMap<CreateMerchantDTO, Merchant>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                //.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                //.ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MerchantStatusEnum.ACTIVE.ToString()));

            // Mapping for UpdateMerchantDTO to Merchant
            //CreateMap<UpdateMerchantDTO, Merchant>()
            //    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MerchantRegisteredDTO, Merchant>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.MerchantName))
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MerchantStatusEnum.UNVERIFIED.ToString())); ;
        }
    }
}

