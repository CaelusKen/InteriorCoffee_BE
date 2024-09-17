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
        Task<(List<Review>, int, int, int)> GetReviewsAsync(int pageNumber, int pageSize);
        Task<Review> GetReviewById(string id);
        Task CreateReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(string id);

        #region Get Function
        Task<Review> GetReview(Expression<Func<Review, bool>> predicate = null,
                                 Expression<Func<Review, object>> orderBy = null);
        Task<TResult> GetReview<TResult>(Expression<Func<Review, TResult>> selector,
                                          Expression<Func<Review, bool>> predicate = null,
                                          Expression<Func<Review, object>> orderBy = null);
        Task<List<Review>> GetReviewList(Expression<Func<Review, bool>> predicate = null,
                                           Expression<Func<Review, object>> orderBy = null);
        Task<List<TResult>> GetReviewList<TResult>(Expression<Func<Review, TResult>> selector,
                                                    Expression<Func<Review, bool>> predicate = null,
                                                    Expression<Func<Review, object>> orderBy = null);
        Task<IPaginate<Review>> GetReviewPagination(Expression<Func<Review, bool>> predicate = null,
                                                      Expression<Func<Review, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetReviewPagination<TResult>(Expression<Func<Review, TResult>> selector,
                                                               Expression<Func<Review, bool>> predicate = null,
                                                               Expression<Func<Review, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
