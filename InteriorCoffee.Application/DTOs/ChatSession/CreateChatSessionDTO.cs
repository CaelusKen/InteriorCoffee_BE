using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.ChatSession
{
    public class CreateChatSessionDTO
    {
        public List<ChatMessage> Messages { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CustomerId { get; set; }
        public string AdvisorId { get; set; }
    }
}
