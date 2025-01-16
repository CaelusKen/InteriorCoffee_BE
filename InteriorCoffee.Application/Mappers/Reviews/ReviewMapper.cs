using AutoMapper;
using InteriorCoffee.Application.DTOs.Review;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Reviews
{
    public class ReviewMapper : Profile
    {
        public ReviewMapper()
        {
            CreateMap<CreateReviewListDTO, Review>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}
