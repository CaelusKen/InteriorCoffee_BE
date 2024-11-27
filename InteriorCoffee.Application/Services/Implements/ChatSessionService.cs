using AutoMapper;
using InteriorCoffee.Application.DTOs.ChatMessage;
using InteriorCoffee.Application.DTOs.ChatSession;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InteriorCoffee.Application.Constants.ApiEndPointConstant;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ChatSessionService : BaseService<ChatSessionService>, IChatSessionService
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IAccountRepository _accountRepository;

        public ChatSessionService(ILogger<ChatSessionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IChatSessionRepository chatSessionRepository, IAccountRepository accountRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _chatSessionRepository = chatSessionRepository;
            _accountRepository = accountRepository;
        }

        #region Chat Session
        public async Task<List<Domain.Models.ChatSession>> GetChatSessionListAsync()
        {
            var chatSessions = await _chatSessionRepository.GetChatSessionList();
            return _mapper.Map<List<Domain.Models.ChatSession>>(chatSessions);
        }

        public async Task<List<Domain.Models.ChatSession>> GetMerhcnatChatSessionListAsync(string id)
        {
            var merchantAccountIds = await _accountRepository
                .GetAccountList(predicate: a => a.MerchantId == id,
                                selector: a => a._id);

            var chatSessions = await _chatSessionRepository.GetChatSessionsByAdvisorIdList(merchantAccountIds);

            return _mapper.Map<List<Domain.Models.ChatSession>>(chatSessions);
        }

        public async Task<List<Domain.Models.ChatSession>> GetManagerChatSessionListAsync()
        {
            var merchantAccountIds = await _accountRepository
                .GetAccountList(predicate: a => a.Role.Equals(AccountRoleEnum.MANAGER.ToString()),
                                selector: a => a._id);

            var chatSessions = await _chatSessionRepository.GetChatSessionsByAdvisorIdList(merchantAccountIds);

            return _mapper.Map<List<Domain.Models.ChatSession>>(chatSessions);
        }

        public async Task<Domain.Models.ChatSession> GetChatSessionByIdAsync(string id)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);
            if (chatSession == null)
            {
                throw new NotFoundException($"Chat session with id {id} not found.");
            }
            return _mapper.Map<Domain.Models.ChatSession>(chatSession);
        }

        public async Task CreateChatSessionAsync(CreateChatSessionDTO createChatSessionDTO)
        {
            var chatSession = _mapper.Map<Domain.Models.ChatSession>(createChatSessionDTO);
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
        #endregion

        #region Chat Message
        public async Task AddSentMessage(string chatSessionId, AddChatMessageDTO message)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(chatSessionId);
            if (chatSession == null)
            {
                throw new NotFoundException($"Chat session with id {chatSessionId} not found.");
            }

            //Add new message to chat session
            ChatMessage newMessage = _mapper.Map<ChatMessage>(message);
            chatSession.Messages.Add(newMessage);

            //Update chat session data
            await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        public async Task UpdateSentMessage(string chatSessionId, UpdateChatMessageDTO message)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(chatSessionId);
            if (chatSession == null) throw new NotFoundException($"Chat session with id {chatSessionId} not found.");

            var oldMessage = chatSession.Messages.Where(m => m._id.Equals(message._id)).FirstOrDefault();
            if (message == null) throw new NotFoundException($"Chat message with id {message._id} not found");

            oldMessage.Message = String.IsNullOrEmpty(message.Message) ? oldMessage.Message : message.Message;
            oldMessage.TimeStamp = DateTime.Now;

            await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        public async Task DeleteSentMessage(string chatSessionId, string messageId)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(chatSessionId);
            if (chatSession == null) throw new NotFoundException($"Chat session with id {chatSessionId} not found.");

            var message = chatSession.Messages.Where(m => m._id.Equals(messageId)).FirstOrDefault();
            if (message == null) throw new NotFoundException($"Chat message with id {messageId} not found");

            chatSession.Messages.Remove(message);

            await _chatSessionRepository.UpdateChatSession(chatSession);
        }
        #endregion
    }
}
