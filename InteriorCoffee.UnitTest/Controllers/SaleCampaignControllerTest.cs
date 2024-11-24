using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.SaleCampaign;
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
    public class SaleCampaignControllerTest
    {
        private readonly ILogger<SaleCampaignController> logger;
        private readonly ISaleCampaignService _saleCampaignService;
        private readonly SaleCampaignController _saleCampaignController;

        public SaleCampaignControllerTest()
        {
            logger = A.Fake<ILogger<SaleCampaignController>>();
            _saleCampaignService = A.Fake<ISaleCampaignService>();
            _saleCampaignController = new SaleCampaignController(logger, _saleCampaignService);
        }

        private static CreateSaleCampaignDTO CreateFakeCreateSaleCampaignDTO() => A.Fake<CreateSaleCampaignDTO>();
        private static UpdateSaleCampaignDTO CreateFakeUpdateSaleCampaignDTO() => A.Fake<UpdateSaleCampaignDTO>();

        #region Get Function Test
        [Fact]
        public async void SaleCampaignController_GetSaleCampaigns_ReturnSaleCampaignList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _saleCampaignController.GetSaleCampaigns(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<SaleCampaign>>();
        }

        [Fact]
        public async void SaleCampaignController_GetSaleCampaignById_ReturnSaleCampaign()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _saleCampaignController.GetSaleCampaignById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<SaleCampaign>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void SaleCampaignController_Create_ReturnSuccess()
        {
            //Arrange
            var createSaleCampaignDto = CreateFakeCreateSaleCampaignDTO();

            //Act
            var result = (OkObjectResult)await _saleCampaignController.CreateSaleCampaign(createSaleCampaignDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void SaleCampaignController_Update_ReturnSuccess()
        {
            //Arrange
            var updateSaleCampaignDto = CreateFakeUpdateSaleCampaignDTO();

            //Act
            var result = (OkObjectResult)await _saleCampaignController.UpdateSaleCampaigns("672d61c84e4eeed22aad9f8b", updateSaleCampaignDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void SaleCampaignController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _saleCampaignController.DeleteSaleCampaigns("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
