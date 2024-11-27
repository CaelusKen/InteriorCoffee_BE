using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.ChatSession
{
    public class UpdateChatSessionDTO
    {
        public List<InteriorCoffee.Domain.Models.Documents.ChatMessage> Messages { get; set; }
        public string? AdvisorId { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
