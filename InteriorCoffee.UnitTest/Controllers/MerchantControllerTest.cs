using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Merchant;
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
    public class MerchantControllerTest
    {
        private readonly ILogger<MerchantController> logger;
        private readonly IMerchantService _merchantService;
        private readonly MerchantController _merchantController;

        public MerchantControllerTest()
        {
            logger = A.Fake<ILogger<MerchantController>>();
            _merchantService = A.Fake<IMerchantService>();
            _merchantController = new MerchantController(logger, _merchantService);
        }

        private static CreateMerchantDTO CreateFakeCreateMerchantDTO() => A.Fake<CreateMerchantDTO>();
        private static UpdateMerchantDTO CreateFakeUpdateMerchantDTO() => A.Fake<UpdateMerchantDTO>();

        #region Get Function Test
        [Fact]
        public async void MerchantController_GetMerchants_ReturnMerchantList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _merchantController.GetMerchants(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<MerchantResponseDTO>();
        }

        [Fact]
        public async void MerchantController_GetMerchantById_ReturnMerchant()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _merchantController.GetMerchantById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Merchant>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void MerchantController_Create_ReturnSuccess()
        {
            //Arrange
            var createMerchantDto = CreateFakeCreateMerchantDTO();

            //Act
            var result = (OkObjectResult)await _merchantController.CreateMerchant(createMerchantDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void MerchantController_Update_ReturnSuccess()
        {
            //Arrange
            var updateMerchantDto = CreateFakeUpdateMerchantDTO();

            //Act
            var result = (OkObjectResult)await _merchantController.UpdateMerchant("672d61c84e4eeed22aad9f8b", updateMerchantDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async void MerchantController_Verified_ReturnSuccess()
        {
            //Arrange
            var updateMerchantDto = CreateFakeUpdateMerchantDTO();

            //Act
            var result = (OkObjectResult)await _merchantController.VerifyMerchant("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void MerchantController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _merchantController.DeleteMerchant("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
