﻿using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<(List<Order>, int, int, int, int)> GetOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy);
        Task<(List<Order>, int, int, int, int)> GetMerchantOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy, string id);
        Task<(List<Order>, int, int, int, int)> GetCustomerOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy, string customerId);
        Task<GetOrderDTO> GetOrderByIdAsync(string id);
        Task<string> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        Task UpdateOrderAsync(string id, UpdateOrderStatusDTO order);
        Task DeleteOrderAsync(string id);
    }
}
