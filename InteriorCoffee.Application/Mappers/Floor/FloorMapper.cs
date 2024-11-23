using AutoMapper;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;

namespace InteriorCoffee.Application.Mappers.Floors
{
    public class FloorMapper : Profile
    {
        public FloorMapper()
        {
            // Mapping for CreateFloorDTO to Floor
            CreateMap<CreateFloorDTO, Floor>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            // Mapping for UpdateFloorDTO to Floor
            CreateMap<UpdateFloorDTO, Floor>()
                .ForMember(dest => dest._id, opt => opt.Ignore()); // Ignore _id since it should not be updated

            CreateMap<FloorDTO, Floor>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}

