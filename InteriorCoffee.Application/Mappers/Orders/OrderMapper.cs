using AutoMapper;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.Enums.Order;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.Orders
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            // Mapping for CreateOrderDTO to Order with custom status mapping
            CreateMap<CreateOrderDTO, Order>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items.ContainsKey("Status") ? context.Items["Status"].ToString() : OrderStatusEnum.CREATED.ToString()));

            // Mapping for Order to Order (for splitting)
            CreateMap<Order, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items.ContainsKey("Status") ? context.Items["Status"].ToString() : src.Status));

            // Mapping for UpdateOrderStatusDTO to Order
            CreateMap<UpdateOrderStatusDTO, Order>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
