using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Order;
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
    public class OrderControllerTest
    {
        private readonly ILogger<OrderController> logger;
        private readonly IOrderService _orderService;
        private readonly OrderController _orderController;

        public OrderControllerTest()
        {
            logger = A.Fake<ILogger<OrderController>>();
            _orderService = A.Fake<IOrderService>();
            _orderController = new OrderController(logger, _orderService);
        }

        private static CreateOrderDTO CreateFakeCreateOrderDTO() => A.Fake<CreateOrderDTO>();
        private static UpdateOrderStatusDTO CreateFakeUpdateOrderStatusDTO() => A.Fake<UpdateOrderStatusDTO>();

        #region Get Function Test
        [Fact]
        public async void OrderController_GetOrders_ReturnOrderList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _orderController.GetOrders(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<Order>>();
        }

        [Fact]
        public async void OrderController_GetOrderById_ReturnOrder()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _orderController.GetOrderById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Order>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void OrderController_Create_ReturnSuccess()
        {
            //Arrange
            var createOrderDto = CreateFakeCreateOrderDTO();

            //Act
            var result = (OkObjectResult)await _orderController.CreateOrder(createOrderDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void OrderController_UpdateOrderStatus_ReturnSuccess()
        {
            //Arrange
            var updateOrderDto = CreateFakeUpdateOrderStatusDTO();

            //Act
            var result = (OkObjectResult)await _orderController.UpdateOrderStatus("672d61c84e4eeed22aad9f8b", updateOrderDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void OrderController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _orderController.DeleteOrder("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
