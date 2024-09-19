using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Review;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class ReviewController : BaseController<ReviewController>
    {
        private readonly IReviewService _reviewService;
        public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService) : base(logger)
        {
            _reviewService = reviewService;
        }

        [HttpGet(ApiEndPointConstant.Review.ReviewsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Review>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all reviews with pagination")]
        public async Task<IActionResult> GetReviews([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (reviews, currentPage, currentPageSize, totalItems, totalPages) = await _reviewService.GetReviewsAsync(pageNo, pageSize);

            var response = new Paginate<Review>
            {
                Items = reviews,
                Page = currentPage,
                Size = currentPageSize,
                TotalPages = totalPages,
                TotalItems = reviews.Count,
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Review.ReviewEndpoint)]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a review by id")]
        public async Task<IActionResult> GetReviewById(string id)
        {
            var result = await _reviewService.GetReviewById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Review.ReviewsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create review")]
        public async Task<IActionResult> CreateReview(CreateReviewDTO review)
        {
            await _reviewService.CreateReview(review);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Review.ReviewEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a review's data")]
        public async Task<IActionResult> UpdateReviews(string id, [FromBody] UpdateReviewDTO updateReview)
        {
            await _reviewService.UpdateReview(id, updateReview);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Review.ReviewEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a review")]
        public async Task<IActionResult> DeleteReviews(string id)
        {
            await _reviewService.DeleteReview(id);
            return Ok("Action success");
        }
    }
}
