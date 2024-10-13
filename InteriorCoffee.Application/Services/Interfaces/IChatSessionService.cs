using InteriorCoffee.Application.DTOs.ChatMessage;
using InteriorCoffee.Application.DTOs.ChatSession;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IChatSessionService
    {
        Task<List<ChatSession>> GetChatSessionListAsync();
        Task<ChatSession> GetChatSessionByIdAsync(string id);
        Task CreateChatSessionAsync(CreateChatSessionDTO chatSession);
        Task UpdateChatSessionAsync(string id, UpdateChatSessionDTO chatSession);
        Task DeleteChatSessionAsync(string id);

        Task AddSentMessage(string chatSessionId, AddChatMessageDTO message);
        Task UpdateSentMessage(string chatSessionId, UpdateChatMessageDTO message);
        Task DeleteSentMessage(string chatSessionId, string messageId);
    }
}
