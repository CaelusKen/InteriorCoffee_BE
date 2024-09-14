using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<(List<Order>, int, int, int, int)> GetOrdersAsync(int? pageNo, int? pageSize);
        Task<Order> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(CreateOrderDTO order);
        Task UpdateOrderAsync(string id, UpdateOrderStatusDTO order);
        Task DeleteOrderAsync(string id);
    }
}
