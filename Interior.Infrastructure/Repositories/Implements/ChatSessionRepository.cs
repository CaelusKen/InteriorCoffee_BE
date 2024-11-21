using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class ChatSessionRepository : BaseRepository<ChatSessionRepository>, IChatSessionRepository
    {
        private readonly IMongoCollection<ChatSession> _chatSessions;
        private readonly ILogger<ChatSessionRepository> _logger;

        public ChatSessionRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<ChatSessionRepository> logger) : base(setting, client)
        {
            _chatSessions = _database.GetCollection<ChatSession>("ChatSession");
            _logger = logger;
        }

        public async Task<List<ChatSession>> GetChatSessionList()
        {
            return await _chatSessions.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<ChatSession> GetChatSessionById(string id)
        {
            return await _chatSessions.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ChatSession>> GetChatSessionsByAdvisorIdList(List<string> ids)
        {
            var filter = Builders<ChatSession>.Filter.In("AdvisorId", ids);
            return await _chatSessions.Find(filter).ToListAsync();
        }

        #region Get Function
        public async Task<ChatSession> GetChatSession(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _chatSessions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetChatSession<TResult>(Expression<Func<ChatSession, TResult>> selector, Expression<Func<ChatSession, bool>> predicate = null,
            Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _chatSessions.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<ChatSession>> GetChatSessionList(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _chatSessions.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetChatSessionList<TResult>(Expression<Func<ChatSession, TResult>> selector, Expression<Func<ChatSession, bool>> predicate = null,
            Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _chatSessions.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<ChatSession>> GetChatSessionPagination(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _chatSessions.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetChatSessionPagination<TResult>(Expression<Func<ChatSession, TResult>> selector, Expression<Func<ChatSession, bool>> predicate = null,
            Expression<Func<ChatSession, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _chatSessions.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _chatSessions.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _chatSessions.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _chatSessions.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateChatSession(ChatSession chatSession)
        {
            await _chatSessions.ReplaceOneAsync(a => a._id == chatSession._id, chatSession);
        }

        public async Task CreateChatSession(ChatSession chatSession)
        {
            await _chatSessions.InsertOneAsync(chatSession);
        }

        public async Task DeleteChatSession(string id)
        {
            FilterDefinition<ChatSession> filterDefinition = Builders<ChatSession>.Filter.Eq("_id", id);
            await _chatSessions.DeleteOneAsync(filterDefinition);
        }
    }
}
