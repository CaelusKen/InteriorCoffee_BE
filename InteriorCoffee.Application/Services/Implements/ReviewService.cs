using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Review;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ReviewService : BaseService<ReviewService>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(ILogger<ReviewService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IReviewRepository reviewRepository) 
            : base(logger, mapper, httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<(List<Review>, int, int, int, int)> GetReviewsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allReviews, totalItems) = await _reviewRepository.GetReviewsAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var reviews = allReviews.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                        .Take(pagination.PageSize)
                                        .ToList();

                return (reviews, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated reviews.");
                return (new List<Review>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }

        public async Task<Review> GetReviewById(string id)
        {
            var result = await _reviewRepository.GetReview(
                predicate: rev => rev._id.Equals(id));

            if(result == null) throw new NotFoundException($"Review id {id} cannot be found");

            return result;
        }

        public async Task CreateReview(CreateReviewDTO createReviewDTO)
        {
            Review newReview = _mapper.Map<Review>(createReviewDTO);
            await _reviewRepository.CreateReview(newReview);
        }

        public async Task UpdateReview(string id, UpdateReviewDTO updateReviewDTO)
        {
            Review review = await _reviewRepository.GetReview(
                predicate: rev => rev._id.Equals(id));

            if (review == null) throw new NotFoundException($"Review id {id} cannot be found");

            //Update review data
            review.Comment = String.IsNullOrEmpty(updateReviewDTO.Comment) ? review.Comment : updateReviewDTO.Comment;
            review.Rating = updateReviewDTO.Rating;

            await _reviewRepository.UpdateReview(review);
        }

        public async Task DeleteReview(string id)
        {
            Review review = await _reviewRepository.GetReview(
                predicate: rev => rev._id.Equals(id));

            if (review == null) throw new NotFoundException($"Review id {id} cannot be found");

            await _reviewRepository.DeleteReview(id);
        }
    }
}
