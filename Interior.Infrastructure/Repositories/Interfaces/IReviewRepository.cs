using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<(List<Review>, int, int, int)> GetReviewsAsync(int pageNumber, int pageSize);
        Task<Review> GetReviewById(string id);
        Task CreateReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(string id);

        public Task<List<Review>> GetReviewListByCondition(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null);
        public Task<Review> GetReviewByCondition(Expression<Func<Review, bool>> predicate = null, Expression<Func<Review, object>> orderBy = null);
    }
}
