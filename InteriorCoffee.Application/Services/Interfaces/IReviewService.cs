using InteriorCoffee.Application.DTOs.Review;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<(List<Review>, int, int, int, int)> GetReviewsAsync(int? pageNo, int? pageSize);
        Task<Review> GetReviewById(string id);
        Task CreateReview(CreateReviewDTO createReviewDTO);
        Task UpdateReview(string id, UpdateReviewDTO updateReviewDTO);
        Task DeleteReview(string id);
    }
}
