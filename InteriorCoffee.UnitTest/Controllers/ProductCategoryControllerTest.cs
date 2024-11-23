using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.ProductCategory;
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
    public class ProductCategoryControllerTest
    {
        private readonly ILogger<ProductCategoryController> logger;
        private readonly IProductCategoryService _productCategoryService;
        private readonly ProductCategoryController _productCategoryController;

        public ProductCategoryControllerTest()
        {
            logger = A.Fake<ILogger<ProductCategoryController>>();
            _productCategoryService = A.Fake<IProductCategoryService>();
            _productCategoryController = new ProductCategoryController(logger, _productCategoryService);
        }

        private static CreateProductCategoryDTO CreateFakeCreateProductCategoryDTO() => A.Fake<CreateProductCategoryDTO>();
        private static UpdateProductCategoryDTO CreateFakeUpdateProductCategoryDTO() => A.Fake<UpdateProductCategoryDTO>();

        #region Get Function Test
        [Fact]
        public async void ProductCategoryController_GetProductCategorys_ReturnProductCategoryList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productCategoryController.GetProductCategories(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<ProductCategory>>();
        }

        [Fact]
        public async void ProductCategoryController_GetProductCategoryById_ReturnProductCategory()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productCategoryController.GetProductCategoryById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void ProductCategoryController_Create_ReturnSuccess()
        {
            //Arrange
            var createProductCategoryDto = CreateFakeCreateProductCategoryDTO();

            //Act
            var result = (OkObjectResult)await _productCategoryController.CreateProductCategory(createProductCategoryDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void ProductCategoryController_Update_ReturnSuccess()
        {
            //Arrange
            var updateProductCategoryDto = CreateFakeUpdateProductCategoryDTO();

            //Act
            var result = (OkObjectResult)await _productCategoryController.UpdateProductCategory("672d61c84e4eeed22aad9f8b", updateProductCategoryDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void ProductCategoryController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _productCategoryController.DeleteProductCategory("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
