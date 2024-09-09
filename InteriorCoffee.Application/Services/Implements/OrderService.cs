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
            return await _orderRepository.GetOrderList();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Order ID cannot be null or empty.");
            return await _orderRepository.GetOrderById(id);
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentException("Order cannot be null.");
            await _orderRepository.CreateOrder(order);
        }

        public async Task UpdateOrderAsync(string id, Order order)
        {
            if (string.IsNullOrEmpty(id) || order == null) throw new ArgumentException("Order ID and data cannot be null or empty.");
            await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrderAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Order ID cannot be null or empty.");
            await _orderRepository.DeleteOrder(id);
        }
    }
}
