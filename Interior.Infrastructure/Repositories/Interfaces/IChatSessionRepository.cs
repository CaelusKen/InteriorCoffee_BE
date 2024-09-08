using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IChatSessionRepository
    {
        Task<List<ChatSession>> GetChatSessionList();
        Task<ChatSession> GetChatSessionById(string id);
        Task CreateChatSession(ChatSession chatSession);
        Task UpdateChatSession(ChatSession chatSession);
        Task DeleteChatSession(string id);

        public Task<List<ChatSession>> GetChatSessionListByCondition(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null);
        public Task<ChatSession> GetChatSessionByCondition(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null);
    }
}
