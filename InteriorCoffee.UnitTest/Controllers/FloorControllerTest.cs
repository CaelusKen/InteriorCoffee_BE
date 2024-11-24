using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.UnitTest.Controllers
{
    public class FloorControllerTest
    {
        private readonly ILogger<FloorController> logger;
        private readonly IFloorService _floorService;
        private readonly FloorController _floorController;

        public FloorControllerTest()
        {
            logger = A.Fake<ILogger<FloorController>>();
            _floorService = A.Fake<IFloorService>();
            _floorController = new FloorController(_floorService, logger);
        }

        private static CreateFloorDTO CreateFakeCreateFloorDTO() => A.Fake<CreateFloorDTO>();
        private static UpdateFloorDTO CreateFakeUpdateFloorDTO() => A.Fake<UpdateFloorDTO>();

        #region Get Function Test
        [Fact]
        public async void FloorController_GetFloorById_ReturnFloor()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _floorController.GetFloorById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Floor>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void FloorController_Create_ReturnSuccess()
        {
            //Arrange
            var createFloorDto = CreateFakeCreateFloorDTO();

            //Act
            var result = (OkObjectResult)await _floorController.CreateFloor(createFloorDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void FloorController_Update_ReturnSuccess()
        {
            //Arrange
            var updateFloorDto = CreateFakeUpdateFloorDTO();

            var jsonUpdatedProduct = JsonConvert.SerializeObject(updateFloorDto);
            var jsonDocument = JsonDocument.Parse(jsonUpdatedProduct);
            JsonElement floor = jsonDocument.RootElement;

            //Act
            var result = (OkObjectResult)await _floorController.UpdateFloor("672d61c84e4eeed22aad9f8b", floor);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void FloorController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _floorController.DeleteFloor("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
