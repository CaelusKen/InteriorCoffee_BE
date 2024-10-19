using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
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

        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "orderdate", "OrderDate" },
            { "status", "Status" },
            { "totalamount", "TotalAmount" },
            { "orderdate", "OrderDate" }
        };

        public async Task<(List<Order>, int, int, int, int)> GetOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allOrders, totalItems) = await _orderRepository.GetOrdersAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var orders = allOrders.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                      .Take(pagination.PageSize)
                                      .ToList();

                // Apply sorting logic only if orderBy is provided
                if (orderBy != null)
                {
                    orders = ApplySorting(orders, orderBy);
                }

                return (orders, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated orders.");
                return (new List<Order>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }

        #region "Sorting"
        private List<Order> ApplySorting(List<Order> orders, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Order).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        orders = orderBy.Ascending
                            ? orders.OrderBy(o => propertyInfo.GetValue(o, null)).ToList()
                            : orders.OrderByDescending(o => propertyInfo.GetValue(o, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Order'.");
                }
            }
            return orders;
        }
        #endregion

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
