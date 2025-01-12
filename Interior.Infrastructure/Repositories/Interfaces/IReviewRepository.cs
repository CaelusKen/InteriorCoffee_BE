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
    public interface IReviewRepository
    {
        Task<(List<Review>, int)> GetReviewsAsync();
        Task<Review> GetReviewById(string id);
        Task CreateReview(Review review);
        Task CreateManyReviews(List<Review> reviews);
        Task UpdateReview(Review review);
        Task DeleteReview(string id);

        #region Get Function
        Task<Review> GetReview(Expression<Func<Review, bool>> predicate = null,
                                 Expression<Func<Review, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetReview<TResult>(Expression<Func<Review, TResult>> selector,
                                          Expression<Func<Review, bool>> predicate = null,
                                          Expression<Func<Review, object>> orderBy = null, bool isAscend = true);
        Task<List<Review>> GetReviewList(Expression<Func<Review, bool>> predicate = null,
                                           Expression<Func<Review, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetReviewList<TResult>(Expression<Func<Review, TResult>> selector,
                                                    Expression<Func<Review, bool>> predicate = null,
                                                    Expression<Func<Review, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<Review>> GetReviewPagination(Expression<Func<Review, bool>> predicate = null,
                                                      Expression<Func<Review, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetReviewPagination<TResult>(Expression<Func<Review, TResult>> selector,
                                                               Expression<Func<Review, bool>> predicate = null,
                                                               Expression<Func<Review, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
