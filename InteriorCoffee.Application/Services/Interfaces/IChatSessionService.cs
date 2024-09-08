using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IChatSessionService
    {
        Task<IEnumerable<ChatSession>> GetAllChatSessionsAsync();
        Task<ChatSession> GetChatSessionByIdAsync(string id);
        Task CreateChatSessionAsync(ChatSession chatSession);
        Task UpdateChatSessionAsync(string id, ChatSession chatSession);
        Task DeleteChatSessionAsync(string id);
    }
}
