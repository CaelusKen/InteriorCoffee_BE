using AutoMapper;
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
                .ForMember(des => des.Products, src => src.MapFrom(src => src.Products == null ? new List<ProductList>() : src.Products))
                .ForMember(des => des.Categories, src => src.MapFrom(src => src.Categories == null ? new List<string>() : src.Categories));

            CreateMap<Template, GetTemplateDTO>();
            CreateMap<Template, TemplateResponseItemDTO>();
        }
    }
}
