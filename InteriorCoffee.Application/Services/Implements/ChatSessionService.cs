using AutoMapper;
using InteriorCoffee.Application.DTOs.ChatSession;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ChatSessionService : BaseService<ChatSessionService>, IChatSessionService
    {
        private readonly IChatSessionRepository _chatSessionRepository;

        public ChatSessionService(ILogger<ChatSessionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IChatSessionRepository chatSessionRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _chatSessionRepository = chatSessionRepository;
        }

        public async Task<List<ChatSession>> GetChatSessionListAsync()
        {
            var chatSessions = await _chatSessionRepository.GetChatSessionList();
            return _mapper.Map<List<ChatSession>>(chatSessions);
        }

        public async Task<ChatSession> GetChatSessionByIdAsync(string id)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);
            if (chatSession == null)
            {
                throw new NotFoundException($"Chat session with id {id} not found.");
            }
            return _mapper.Map<ChatSession>(chatSession);
        }

        public async Task CreateChatSessionAsync(CreateChatSessionDTO createChatSessionDTO)
        {
            var chatSession = _mapper.Map<ChatSession>(createChatSessionDTO);
            await _chatSessionRepository.CreateChatSession(chatSession);
        }

        public async Task UpdateChatSessionAsync(string id, UpdateChatSessionDTO updateChatSessionDTO)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);
            if (chatSession == null)
            {
                throw new NotFoundException($"Chat session with id {id} not found.");
            }
            _mapper.Map(updateChatSessionDTO, chatSession);
            await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        public async Task DeleteChatSessionAsync(string id)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);
            if (chatSession == null)
            {
                throw new NotFoundException($"Chat session with id {id} not found.");
            }
            await _chatSessionRepository.DeleteChatSession(id);
        }
    }
}
