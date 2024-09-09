using InteriorCoffee.Domain.Models;
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

        #region Conditional Get
        public async Task<List<Order>> GetOrderListByCondition(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _orders.Find(filter).SortBy(orderBy).ToListAsync();

            return await _orders.Find(filter).ToListAsync();
        }

        public async Task<Order> GetOrderByCondition(Expression<Func<Order, bool>> predicate = null, Expression<Func<Order, object>> orderBy = null)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (predicate != null) filter = filterBuilder.Where(predicate);

            if (orderBy != null) return await _orders.Find(filter).SortBy(orderBy).FirstOrDefaultAsync();

            return await _orders.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<List<Order>> GetOrderList()
        {
            return await _orders.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _orders.Find(c => c._id == id).FirstOrDefaultAsync();
        }

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
