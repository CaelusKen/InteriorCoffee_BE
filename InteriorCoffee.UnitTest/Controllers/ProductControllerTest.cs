using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Controllers;
using InteriorCoffeeAPIs.Validate;
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
    public class ProductControllerTest
    {
        private readonly ILogger<ProductController> logger;
        private readonly ILogger<JsonValidationService> validationLogger;
        private readonly IProductService _productService;
        private readonly ProductController _productController;
        private readonly IMapper _mapper;
        private readonly IDictionary<string, JsonValidationService> _validationServicesDict;

        public ProductControllerTest()
        {
            validationLogger = A.Fake<ILogger<JsonValidationService>>();
            _validationServicesDict = new Dictionary<string, JsonValidationService>();
            SetupJsonValidation();

            logger = A.Fake<ILogger<ProductController>>();
            _productService = A.Fake<IProductService>();
            _productController = new ProductController(logger, _productService, _validationServicesDict, _mapper);
        }

        private void SetupJsonValidation()
        {
            var schemaFilePath = GetFilePath();

            var validationService = new JsonValidationService(schemaFilePath, validationLogger);
            var schemaName = "ProductValidate";
            _validationServicesDict[schemaName] = validationService;
        }

        private string GetFilePath()
        {
            var schemaFilePath = Path.GetFullPath("ProductValidate.json");
            var removeIndex = schemaFilePath.IndexOf("bin");
            var result = schemaFilePath.Substring(0, removeIndex);
            result = result + @"ValidationFile\ProductValidate.json";

            return result;
        }

        private static CreateProductDTO CreateFakeCreateProductDTO() => A.Fake<CreateProductDTO>();
        private static UpdateProductDTO CreateFakeUpdateProductDTO() => A.Fake<UpdateProductDTO>();

        #region Get Function Test
        [Fact]
        public async void ProductController_GetProducts_ReturnProductList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productController.GetProducts(null, null, null, null, null, null, null, null, null, null, null);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<ProductResponseDTO>();
        }

        [Fact]
        public async void ProductController_GetProductById_ReturnProduct()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productController.GetProductById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Domain.Models.Product>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void ProductController_Create_ReturnSuccess()
        {
            //Arrange
            var createProductDto = CreateFakeCreateProductDTO();
            createProductDto.CategoryIds = new List<string> { "1", "2" };
            createProductDto.Name = "Test";
            createProductDto.TruePrice = 100000;
            createProductDto.Discount = 10;
            createProductDto.Quantity = 100;
            createProductDto.MerchantId = "Test";
            createProductDto.Materials = new List<string>();
            createProductDto.Description = string.Empty;
            createProductDto.Images = new ProductImages() { NormalImages = new List<string>(), Thumbnail = string.Empty };
            createProductDto.Dimensions = string.Empty;
            createProductDto.ModelTextureUrl = string.Empty;
            createProductDto.CampaignId = string.Empty;

            //Act
            var result = (OkObjectResult)await _productController.CreateProduct(createProductDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void ProductController_Update_ReturnSuccess()
        {
            //Arrange
            //Set data
            var updateProductDto = CreateFakeUpdateProductDTO();
            updateProductDto.CategoryIds = new List<string> { "1", "2" };
            updateProductDto.Name = "Test";
            updateProductDto.TruePrice = 100000;
            updateProductDto.Discount = 10;

            var jsonUpdatedProduct = JsonConvert.SerializeObject(updateProductDto);
            var jsonDocument = JsonDocument.Parse(jsonUpdatedProduct);
            JsonElement product = jsonDocument.RootElement;

            //Act
            var result = (OkObjectResult)await _productController.UpdateProduct("672d61c84e4eeed22aad9f8b", product);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void ProductController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productController.DeleteProduct("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}