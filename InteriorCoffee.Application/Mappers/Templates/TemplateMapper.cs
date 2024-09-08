﻿using AutoMapper;
using InteriorCoffee.Application.DTOs.Template;
using InteriorCoffee.Application.Enums.Template;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Templates
{
    public class TemplateMapper : Profile
    {
        public TemplateMapper()
        {
            CreateMap<CreateTemplateDTO, Template>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(des => des.CreatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.Now))
                .ForMember(des => des.Status, src => src.MapFrom(src => TemplateStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des.Floors, src => src.MapFrom(src => new List<Floor>()));
        }
    }
}
