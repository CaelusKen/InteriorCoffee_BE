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
    public class ReviewRepository : BaseRepository<ReviewRepository>, IReviewRepository
    {
        private readonly IMongoCollection<Review> _reviews;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<ReviewRepository> logger) : base(setting, client)
        {
            _reviews = _database.GetCollection<Review>("Review");
            _logger = logger;
        }

        public async Task<(List<Review>, int, int, int)> GetReviewsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalItemsLong = await _reviews.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var reviews = await _reviews.Find(new BsonDocument())
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Limit(pageSize)
                                            .ToListAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return (reviews, totalItems, pageSize, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated reviews.");
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

        #region Get Function
        public async Task<Review> GetReview(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _reviews.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetReview<TResult>(Expression<Func<Review, TResult>> selector, Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();

            return await _reviews.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Review>> GetReviewList(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).ToListAsync();

            return await _reviews.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetReviewList<TResult>(Expression<Func<Review, TResult>> selector, Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();

            return await _reviews.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Review>> GetReviewPagination(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);

            return await _reviews.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetReviewPagination<TResult>(Expression<Func<Review, TResult>> selector, Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Review>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _reviews.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            return await _reviews.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

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
    }
}
