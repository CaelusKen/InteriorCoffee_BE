using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ChatSessionService : IChatSessionService
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly ILogger<ChatSessionService> _logger;

        public ChatSessionService(IChatSessionRepository chatSessionRepository, ILogger<ChatSessionService> logger)
        {
            _chatSessionRepository = chatSessionRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ChatSession>> GetAllChatSessionsAsync()
        {
            return await _chatSessionRepository.GetChatSessionList();
        }

        public async Task<ChatSession> GetChatSessionByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Chat session ID cannot be null or empty.");

            return await _chatSessionRepository.GetChatSessionById(id);
        }

        public async Task CreateChatSessionAsync(ChatSession chatSession)
        {
            if (chatSession == null) throw new ArgumentException("Chat session cannot be null.");
            await _chatSessionRepository.CreateChatSession(chatSession);
        }

        public async Task UpdateChatSessionAsync(string id, ChatSession chatSession)
        {
            if (string.IsNullOrEmpty(id) || chatSession == null) throw new ArgumentException("Chat session ID and data cannot be null or empty.");
            await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        public async Task DeleteChatSessionAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Chat session ID cannot be null or empty.");
            await _chatSessionRepository.DeleteChatSession(id);
        }
    }
}
