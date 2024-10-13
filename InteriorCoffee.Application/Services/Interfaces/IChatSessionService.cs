﻿using InteriorCoffee.Application.DTOs.ChatSession;
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

        Task AddSentMessage(string chattSessionId, ChatMessageDTO message);
    }
}
