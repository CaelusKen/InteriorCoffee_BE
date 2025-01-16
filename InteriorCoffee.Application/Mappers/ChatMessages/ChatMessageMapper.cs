using AutoMapper;
using InteriorCoffee.Application.DTOs.ChatMessage;
using InteriorCoffee.Domain.Models.Documents;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.ChatMessages
{
    public class ChatMessageMapper : Profile
    {
        public ChatMessageMapper()
        {
            CreateMap<AddChatMessageDTO, ChatMessage>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(des => des.TimeStamp, src => src.MapFrom(src => DateTime.Now));
        }
    }
}
