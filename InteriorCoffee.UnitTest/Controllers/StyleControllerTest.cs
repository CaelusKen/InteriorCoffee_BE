using Amazon.Runtime.Internal.Util;
using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Style;
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
    public class StyleControllerTest
    {
        private readonly ILogger<StyleController> logger;
        private readonly IStyleService _styleService;
        private readonly StyleController _styleController;

        public StyleControllerTest()
        {
            logger = A.Fake<ILogger<StyleController>>();
            _styleService = A.Fake<IStyleService>();
            _styleController = new StyleController(logger, _styleService);
        }

        private static StyleDTO CreateFakeStyleDTO() => A.Fake<StyleDTO>();

        #region Get Function Test
        [Fact]
        public async void StyleController_GetStyles_ReturnStyleList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _styleController.GetStyles(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<Style>>();
        }

        [Fact]
        public async void StyleController_GetStyleById_ReturnStyle()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _styleController.GetStyleById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Style>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void StyleController_Create_ReturnSuccess()
        {
            //Arrange
            var createStyleDto = CreateFakeStyleDTO();

            //Act
            var result = (OkObjectResult)await _styleController.CreateStyle(createStyleDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void StyleController_Update_ReturnSuccess()
        {
            //Arrange
            var updateStyleDto = CreateFakeStyleDTO();

            //Act
            var result = (OkObjectResult)await _styleController.UpdateStyles("672d61c84e4eeed22aad9f8b", updateStyleDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void StyleController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _styleController.DeleteStyles("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
