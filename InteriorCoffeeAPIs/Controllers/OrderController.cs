using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
        [ProducesResponseType(typeof(IPaginate<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get all orders with pagination and sorting")]
        public async Task<IActionResult> GetOrders([FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null)
        {
            //try
            //{
                OrderBy orderBy = null;
                if (!string.IsNullOrEmpty(sortBy))
                {
                    orderBy = new OrderBy(sortBy, ascending ?? true);
                }

                var (orders, currentPage, currentPageSize, totalItems, totalPages) = await _orderService.GetOrdersAsync(pageNo, pageSize, orderBy);

                var response = new Paginate<Order>
                {
                    Items = orders,
                    PageNo = currentPage,
                    PageSize = currentPageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                };

                return Ok(response);
            //}
            //catch (ArgumentException ex)
            //{
            //    _logger.LogError(ex, "Invalid argument provided.");
            //    return BadRequest(new { message = ex.Message });
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An error occurred while processing your request.");
            //    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            //}
        }

        [HttpGet(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get an order by id")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Order.MerchantOrdersEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Order>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a merchant's orders")]
        public async Task<IActionResult> GetOrderByMerchantId(string id, [FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null)
        {
            OrderBy orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                orderBy = new OrderBy(sortBy, ascending ?? true);
            }

            var (orders, currentPage, currentPageSize, totalItems, totalPages) = await _orderService.GetMerchantOrdersAsync(pageNo, pageSize, orderBy, id);

            var response = new Paginate<Order>
            {
                Items = orders,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
            };

            return Ok(response);
        }

        [HttpPost(ApiEndPointConstant.Order.OrdersEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create order")]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO order)
        {
            var orderId = await _orderService.CreateOrderAsync(order);
            return Ok(orderId);
        }

        [HttpPatch(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an order's status")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] UpdateOrderStatusDTO updateOrderStatus)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id);

            await _orderService.UpdateOrderAsync(id, updateOrderStatus);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete an order")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            await _orderService.DeleteOrderAsync(id);
            return Ok("Action success");
        }
    }
}
