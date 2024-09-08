using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetOrderList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all orders.");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid order ID.");
                throw new ArgumentException("Order ID cannot be null or empty.");
            }

            try
            {
                return await _orderRepository.GetOrderById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting order with id {id}.");
                throw;
            }
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogWarning("Invalid order data.");
                throw new ArgumentException("Order cannot be null.");
            }

            try
            {
                await _orderRepository.CreateOrder(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an order.");
                throw;
            }
        }

        public async Task UpdateOrderAsync(string id, Order order)
        {
            if (string.IsNullOrEmpty(id) || order == null)
            {
                _logger.LogWarning("Invalid order ID or data.");
                throw new ArgumentException("Order ID and data cannot be null or empty.");
            }

            try
            {
                await _orderRepository.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating order with id {id}.");
                throw;
            }
        }

        public async Task DeleteOrderAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid order ID.");
                throw new ArgumentException("Order ID cannot be null or empty.");
            }

            try
            {
                await _orderRepository.DeleteOrder(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting order with id {id}.");
                throw;
            }
        }
    }
}
