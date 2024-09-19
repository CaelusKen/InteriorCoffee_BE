using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
        Task<(List<Order>, int)> GetOrdersAsync();
        Task<Order> GetOrderById(string id);
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(string id);

        #region Get Function
        Task<Order> GetOrder(Expression<Func<Order, bool>> predicate = null,
                                 Expression<Func<Order, object>> orderBy = null, bool isAscend = true);
        Task<TResult> GetOrder<TResult>(Expression<Func<Order, TResult>> selector,
                                          Expression<Func<Order, bool>> predicate = null,
                                          Expression<Func<Order, object>> orderBy = null, bool isAscend = true);
        Task<List<Order>> GetOrderList(Expression<Func<Order, bool>> predicate = null,
                                           Expression<Func<Order, object>> orderBy = null, bool isAscend = true);
        Task<List<TResult>> GetOrderList<TResult>(Expression<Func<Order, TResult>> selector,
                                                    Expression<Func<Order, bool>> predicate = null,
                                                    Expression<Func<Order, object>> orderBy = null, bool isAscend = true);
        Task<IPaginate<Order>> GetOrderPagination(Expression<Func<Order, bool>> predicate = null,
                                                      Expression<Func<Order, object>> orderBy = null, bool isAscend = true,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetOrderPagination<TResult>(Expression<Func<Order, TResult>> selector,
                                                               Expression<Func<Order, bool>> predicate = null,
                                                               Expression<Func<Order, object>> orderBy = null, bool isAscend = true,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
