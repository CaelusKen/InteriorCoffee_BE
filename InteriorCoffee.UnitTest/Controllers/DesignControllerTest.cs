using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Design;
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
    public class DesignControllerTest
    {
        private readonly ILogger<DesignController> logger;
        private readonly IDesignService _designService;
        private readonly DesignController _designController;

        public DesignControllerTest()
        {
            logger = A.Fake<ILogger<DesignController>>();
            _designService = A.Fake<IDesignService>();
            _designController = new DesignController(logger, _designService);
        }

        private static CreateDesignDTO CreateFakeCreateDesignDTO() => A.Fake<CreateDesignDTO>();
        private static UpdateDesignDTO CreateFakeUpdateDesignDTO() => A.Fake<UpdateDesignDTO>();

        #region Get Function Test
        [Fact]
        public async void DesignController_GetDesigns_ReturnDesignList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _designController.GetDesigns(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<DesignResponseDTO>();
        }

        [Fact]
        public async void DesignController_GetDesignById_ReturnDesign()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _designController.GetDesignById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<GetDesignDTO>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void DesignController_Create_ReturnSuccess()
        {
            //Arrange
            var createDesignDto = CreateFakeCreateDesignDTO();

            //Act
            var result = (OkObjectResult)await _designController.CreateDesign(createDesignDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void DesignController_Update_ReturnSuccess()
        {
            //Arrange
            var updateDesignDto = CreateFakeUpdateDesignDTO();

            //Act
            var result = (OkObjectResult)await _designController.UpdateDesign("672d61c84e4eeed22aad9f8b", updateDesignDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void DesignController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _designController.DeleteDesign("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
