using AutoMapper;
using InteriorCoffee.Application.DTOs.Style;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Styles
{
    public class StyleMapper : Profile
    {
        public StyleMapper()
        {
            CreateMap<StyleDTO, Style>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}
