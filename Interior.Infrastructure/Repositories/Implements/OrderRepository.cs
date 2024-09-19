using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class OrderRepository : BaseRepository<OrderRepository>, IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(IOptions<MongoDBContext> setting, IMongoClient client, ILogger<OrderRepository> logger) : base(setting, client)
        {
            _orders = _database.GetCollection<Order>("Order");
            _logger = logger;
        }

        public async Task<(List<Order>, int)> GetOrdersAsync()
        {
            try
            {
                var totalItemsLong = await _orders.CountDocumentsAsync(new BsonDocument());
                var totalItems = (int)totalItemsLong;
                var orders = await _orders.Find(new BsonDocument()).ToListAsync();
                return (orders, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting orders.");
                throw;
            }
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _orders.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        #region Get Function
        public async Task<Order> GetOrder(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).FirstOrDefaultAsync();
            }

            return await _orders.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetOrder<TResult>(Expression<Func<Order, TResult>> selector, Expression<Func<Order, bool>> predicate = null,
            Expression<Func<Order, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).Project(selector).FirstOrDefaultAsync();
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).Project(selector).FirstOrDefaultAsync();
            }

            return await _orders.Find(filter).Project(selector).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrderList(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).ToListAsync();
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).ToListAsync();
            }

            return await _orders.Find(filter).ToListAsync();
        }

        public async Task<List<TResult>> GetOrderList<TResult>(Expression<Func<Order, TResult>> selector, Expression<Func<Order, bool>> predicate = null,
            Expression<Func<Order, object>> orderBy = null, bool isAscend = true)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).Project(selector).ToListAsync();
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).Project(selector).ToListAsync();
            }

            return await _orders.Find(filter).Project(selector).ToListAsync();
        }

        public async Task<IPaginate<Order>> GetOrderPagination(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null,
            bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).ToPaginateAsync(page, size, 1);
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).ToPaginateAsync(page, size, 1);
            }

            return await _orders.Find(filter).ToPaginateAsync(page, size, 1);
        }

        public async Task<IPaginate<TResult>> GetOrderPagination<TResult>(Expression<Func<Order, TResult>> selector, Expression<Func<Order, bool>> predicate = null,
            Expression<Func<Order, object>> orderBy = null, bool isAscend = true, int page = 1, int size = 10)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _orders.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);

            if (orderBy != null)
            {
                if (isAscend)
                    return await _orders.Find(filter).SortBy(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
                else
                    return await _orders.Find(filter).SortByDescending(orderBy).Project(selector).ToPaginateAsync(page, size, 1);
            }

            return await _orders.Find(filter).Project(selector).ToPaginateAsync(page, size, 1);
        }
        #endregion

        public async Task UpdateOrder(Order order)
        {
            await _orders.ReplaceOneAsync(a => a._id == order._id, order);
        }

        public async Task CreateOrder(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task DeleteOrder(string id)
        {
            FilterDefinition<Order> filterDefinition = Builders<Order>.Filter.Eq("_id", id);
            await _orders.DeleteOneAsync(filterDefinition);
        }
    }
}
