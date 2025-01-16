using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
        Task<List<ChatSession>> GetChatSessionsByAdvisorIdList(List<string> ids);
        Task CreateChatSession(ChatSession chatSession);
        Task UpdateChatSession(ChatSession chatSession);
        Task DeleteChatSession(string id);

        #region Get Function
        Task<ChatSession> GetChatSession(Expression<Func<ChatSession, bool>> predicate = null,
                                 Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetChatSession<TResult>(Expression<Func<ChatSession, TResult>> selector,
                                          Expression<Func<ChatSession, bool>> predicate = null,
                                          Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true);
        Task<List<ChatSession>> GetChatSessionList(Expression<Func<ChatSession, bool>> predicate = null,
                                           Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetChatSessionList<TResult>(Expression<Func<ChatSession, TResult>> selector,
                                                    Expression<Func<ChatSession, bool>> predicate = null,
                                                    Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<ChatSession>> GetChatSessionPagination(Expression<Func<ChatSession, bool>> predicate = null,
                                                      Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetChatSessionPagination<TResult>(Expression<Func<ChatSession, TResult>> selector,
                                                               Expression<Func<ChatSession, bool>> predicate = null,
                                                               Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
