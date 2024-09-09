using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [HttpGet(ApiEndPointConstant.Order.OrdersEndpoint)]
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetOrderListAsync();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get an order by id")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Order.OrdersEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create order")]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO order)
        {
            await _orderService.CreateOrderAsync(order);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an order's status")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] UpdateOrderStatusDTO updateOrderStatus)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderService.UpdateOrderAsync(id, updateOrderStatus);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete an order")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(id);
            return Ok("Action success");
        }
    }
}
