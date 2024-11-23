using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Review;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.UnitTest.Controllers
{
    public class ReviewControllerTest
    {
        private readonly ILogger<ReviewController> logger;
        private readonly IReviewService _reviewService;
        private readonly ReviewController _reviewController;

        public ReviewControllerTest()
        {
            logger = A.Fake<ILogger<ReviewController>>();
            _reviewService = A.Fake<IReviewService>();
            _reviewController = new ReviewController(logger, _reviewService);
        }

        private static CreateReviewDTO CreateFakeCreateReviewDTO() => A.Fake<CreateReviewDTO>();
        private static UpdateReviewDTO CreateFakeUpdateReviewDTO() => A.Fake<UpdateReviewDTO>();

        #region Get Function Test
        [Fact]
        public async void ReviewController_GetReviews_ReturnReviewList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _reviewController.GetReviews(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<Review>>();
        }

        [Fact]
        public async void ReviewController_GetReviewById_ReturnReview()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _reviewController.GetReviewById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void ReviewController_Create_ReturnSuccess()
        {
            //Arrange
            var createReviewDto = CreateFakeCreateReviewDTO();

            //Act
            var result = (OkObjectResult)await _reviewController.CreateReview(createReviewDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void ReviewController_Update_ReturnSuccess()
        {
            //Arrange
            var updateReviewDto = CreateFakeUpdateReviewDTO();

            //Act
            var result = (OkObjectResult)await _reviewController.UpdateReviews("672d61c84e4eeed22aad9f8b", updateReviewDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void ReviewController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _reviewController.DeleteReviews("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
