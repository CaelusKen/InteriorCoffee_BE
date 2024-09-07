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

        #region Conditional Get
        public async Task<List<Review>> GetReviewListByCondition(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).ToListAsync();

            return await _reviews.Find(filter).ToListAsync();
        }

        public async Task<Review> GetReviewByCondition(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _reviews.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Review>> GetReviewList()
        {
            try
            {
                return await _reviews.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting review list.");
                throw;
            }
        }

        public async Task<Review> GetReviewById(string id)
        {
            try
            {
                return await _reviews.Find(c => c._id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting review with id {id}.");
                throw;
            }
        }

        public async Task UpdateReview(Review review)
        {
            try
            {
                await _reviews.ReplaceOneAsync(a => a._id == review._id, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating review with id {review._id}.");
                throw;
            }
        }

        public async Task CreateReview(Review review)
        {
            try
            {
                await _reviews.InsertOneAsync(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an review.");
                throw;
            }
        }

        public async Task DeleteReview(string id)
        {
            try
            {
                FilterDefinition<Review> filterDefinition = Builders<Review>.Filter.Eq("_id", id);
                await _reviews.DeleteOneAsync(filterDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting review with id {id}.");
                throw;
            }
        }
    }
}
