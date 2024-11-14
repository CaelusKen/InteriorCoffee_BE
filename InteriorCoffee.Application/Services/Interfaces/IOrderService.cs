using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<(List<Order>, int, int, int, int)> GetOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy);
        Task<Order> GetOrderByIdAsync(string id);
        Task<string> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        Task UpdateOrderAsync(string id, UpdateOrderStatusDTO order);
        Task DeleteOrderAsync(string id);
    }
}
