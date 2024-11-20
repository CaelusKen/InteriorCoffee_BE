using AutoMapper;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Floor
{
    public class FloorMapper : Profile
    {
        public FloorMapper()
        {
            // Mapping for CreateFloorDTO to Floor
            CreateMap<CreateFloorDTO, Domain.Models.Floor>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            // Mapping for UpdateFloorDTO to Floor
            CreateMap<UpdateFloorDTO, Domain.Models.Floor>();

            // Additional mappings can be added as needed
        }
    }
}
