using AutoMapper;
using InteriorCoffee.Application.DTOs.ChatSession;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;

namespace InteriorCoffee.Application.Mappers.ChatSessions
{
    public class ChatSessionMapper : Profile
    {
        public ChatSessionMapper()
        {
            // Mapping for CreateChatSessionDTO to ChatSession
            CreateMap<CreateChatSessionDTO, ChatSession>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping for UpdateChatSessionDTO to ChatSession
            CreateMap<UpdateChatSessionDTO, ChatSession>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
