using AutoMapper;
using InteriorCoffee.Application.DTOs.ProductCategory;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.ProductCategories
{
    public class ProductCategoryMapper : Profile
    {
        public ProductCategoryMapper()
        {
            // Mapping for CreateProductCategoryDTO to ProductCategory
            CreateMap<CreateProductCategoryDTO, ProductCategory>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));

            // Mapping for UpdateProductCategoryDTO to ProductCategory
            CreateMap<UpdateProductCategoryDTO, ProductCategory>();
        }
    }
}
