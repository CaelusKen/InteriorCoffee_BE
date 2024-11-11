using AutoMapper;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Enums.Product;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Products
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            // Mapping for CreateProductDTO to Product
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.TruePrice * (1 - src.Discount)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProductStatusEnum.ACTIVE.ToString()));

            // Mapping for UpdateProductDTO to Product
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping for ProductResponseItemDTO to Product
            CreateMap<Product, ProductResponseItemDTO>();
        }
    }
}
