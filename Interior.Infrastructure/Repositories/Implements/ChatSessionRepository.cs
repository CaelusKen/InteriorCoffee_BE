using InteriorCoffee.Domain.Models;
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

        #region Conditional Get
        public async Task<List<ChatSession>> GetChatSessionListByCondition(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _chatSessions.Find(filter).SortBy(orderBy).ToListAsync();

            return await _chatSessions.Find(filter).ToListAsync();
        }

        public async Task<ChatSession> GetChatSessionByCondition(Expression<Func<ChatSession, bool>> predicate = null, Expression<Func<ChatSession, object>> orderBy = null)
        {
            var filterBuilder = Builders<ChatSession>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _chatSessions.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _chatSessions.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<ChatSession>> GetChatSessionList()
        {
            return await _chatSessions.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<ChatSession> GetChatSessionById(string id)
        {
            return await _chatSessions.Find(c => c._id == id).FirstOrDefaultAsync();
        }

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
