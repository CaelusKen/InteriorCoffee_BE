using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Template;
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
    public class TemplateControllerTest
    {
        private readonly ILogger<TemplateController> logger;
        private readonly ITemplateService _templateService;
        private readonly TemplateController _templateController;

        public TemplateControllerTest()
        {
            logger = A.Fake<ILogger<TemplateController>>();
            _templateService = A.Fake<ITemplateService>();
            _templateController = new TemplateController(logger, _templateService);
        }

        private static CreateTemplateDTO CreateFakeCreateTemplateDTO() => A.Fake<CreateTemplateDTO>();
        private static UpdateTemplateDTO CreateFakeUpdateTemplateDTO() => A.Fake<UpdateTemplateDTO>();

        #region Get Function Test
        [Fact]
        public async void TemplateController_GetTemplates_ReturnTemplateList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _templateController.GetTemplates(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<TemplateResponseDTO>();
        }

        [Fact]
        public async void TemplateController_GetTemplateById_ReturnTemplate()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _templateController.GetTemplateById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<GetTemplateDTO>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void TemplateController_Create_ReturnSuccess()
        {
            //Arrange
            var createTemplateDto = CreateFakeCreateTemplateDTO();

            //Act
            var result = (OkObjectResult)await _templateController.CreateTemplate(createTemplateDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void TemplateController_Update_ReturnSuccess()
        {
            //Arrange
            var updateTemplateDto = CreateFakeUpdateTemplateDTO();

            //Act
            var result = (OkObjectResult)await _templateController.UpdateTemplates("672d61c84e4eeed22aad9f8b", updateTemplateDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void TemplateController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _templateController.DeleteTemplates("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
