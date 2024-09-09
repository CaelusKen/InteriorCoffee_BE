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
    public class ReviewRepository : BaseRepository<ReviewRepository>, IReviewRepository
    {
        private readonly IMongoCollection<Review> _reviews;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<ReviewRepository> logger) : base(setting, client)
        {
            _reviews = _database.GetCollection<Review>("Review");
            _logger = logger;
        }

        #region CRUD Functions
        public async Task<List<Review>> GetReviewList(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).ToListAsync();

            return await _reviews.Find(filter).ToListAsync();
        }

        public async Task<Review> GetReview(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _reviews.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateReview(Review review)
        {
            await _reviews.ReplaceOneAsync(a => a._id == review._id, review);
        }

        public async Task CreateReview(Review review)
        {
            await _reviews.InsertOneAsync(review);
        }

        public async Task DeleteReview(string id)
        {
            FilterDefinition<Review> filterDefinition = Builders<Review>.Filter.Eq("_id", id);
            await _reviews.DeleteOneAsync(filterDefinition);
        }
        #endregion 
    }
}
