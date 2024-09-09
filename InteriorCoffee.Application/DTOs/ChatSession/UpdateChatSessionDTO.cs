using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.ChatSession
{
    public class UpdateChatSessionDTO
    {
        public List<ChatMessage> Messages { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
