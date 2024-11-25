using AutoMapper;
using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Application.Enums.Design;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Designs
{
    public class DesignMapper : Profile
    {
        public DesignMapper()
        {
            // Mapping for CreateDesignDTO to Design
            CreateMap<CreateDesignDTO, Design>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DesignStatusEnum.ACTIVE.ToString()))
                .AfterMap((src, des) => src.Floors.ForEach(f => f.DesignTemplateId = des._id) );

            // Mapping for UpdateDesignDTO to Design
            CreateMap<UpdateDesignDTO, Design>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            // Mapping for Design to GetDesignDTO
            CreateMap<Design, GetDesignDTO>();

            // Mapping for Design to DesignResponseItemDTO
            CreateMap<Design, DesignResponseItemDTO>();
        }
    }
}
