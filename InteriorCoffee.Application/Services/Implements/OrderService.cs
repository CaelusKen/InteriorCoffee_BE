using AutoMapper;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> GetOrderListAsync()
        {
            var orders = await _orderRepository.GetOrderList();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
            return order;
        }

        public async Task CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            var order = _mapper.Map<Order>(createOrderDTO);
            await _orderRepository.CreateOrder(order);
        }

        public async Task UpdateOrderAsync(string id, UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var existingOrder = await _orderRepository.GetOrderById(id);
            if (existingOrder == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
            _mapper.Map(updateOrderStatusDTO, existingOrder);
            await _orderRepository.UpdateOrder(existingOrder);
        }

        public async Task DeleteOrderAsync(string id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
            await _orderRepository.DeleteOrder(id);
        }
    }
}
