using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<(List<Order>, int, int, int)> GetOrdersAsync(int pageNumber, int pageSize);
        Task<Order> GetOrderById(string id);
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(string id);

        public Task<List<Order>> GetOrderListByCondition(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null);
        public Task<Order> GetOrderByCondition(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null);
    }
}
