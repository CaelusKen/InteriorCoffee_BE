using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.ChatMessage
{
    public class AddChatMessageDTO
    {
        public string SenderId { get; set; }
        public string Message { get; set; }
    }
}
