using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.ChatSession
{
    public class CreateChatSessionDTO
    {
        public List<InteriorCoffee.Domain.Models.Documents.ChatMessage> Messages { get; set; }
        //public DateTime CreatedDate { get; set; }
        public string CustomerId { get; set; } = null!;
        public string AdvisorId { get; set; } = null!;
    }
}
