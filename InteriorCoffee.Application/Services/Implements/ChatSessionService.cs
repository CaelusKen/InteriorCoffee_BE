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
            try
            {
                return await _chatSessionRepository.GetChatSessionList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all chat sessions.");
                throw;
            }
        }

        public async Task<ChatSession> GetChatSessionByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid chat session ID.");
                throw new ArgumentException("Chat session ID cannot be null or empty.");
            }

            try
            {
                return await _chatSessionRepository.GetChatSessionById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting chat session with id {id}.");
                throw;
            }
        }

        public async Task CreateChatSessionAsync(ChatSession chatSession)
        {
            if (chatSession == null)
            {
                _logger.LogWarning("Invalid chat session data.");
                throw new ArgumentException("Chat session cannot be null.");
            }

            try
            {
                await _chatSessionRepository.CreateChatSession(chatSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a chat session.");
                throw;
            }
        }

        public async Task UpdateChatSessionAsync(string id, ChatSession chatSession)
        {
            if (string.IsNullOrEmpty(id) || chatSession == null)
            {
                _logger.LogWarning("Invalid chat session ID or data.");
                throw new ArgumentException("Chat session ID and data cannot be null or empty.");
            }

            try
            {
                await _chatSessionRepository.UpdateChatSession(chatSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating chat session with id {id}.");
                throw;
            }
        }

        public async Task DeleteChatSessionAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid chat session ID.");
                throw new ArgumentException("Chat session ID cannot be null or empty.");
            }

            try
            {
                await _chatSessionRepository.DeleteChatSession(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting chat session with id {id}.");
                throw;
            }
        }
    }
}
