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
        public Task<List<Review>> GetAllReviews();
        public Task<Review> GetReviewById(string id);
        public Task CreateReview(CreateReviewDTO createReviewDTO);
        public Task UpdateReview(string id, UpdateReviewDTO updateReviewDTO);
        public Task DeleteReview(string id);
    }
}
