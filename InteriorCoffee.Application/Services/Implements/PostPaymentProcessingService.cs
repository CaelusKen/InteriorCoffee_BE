using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using InteriorCoffee.Application.Enums.Order;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models.Documents;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;

namespace InteriorCoffee.Application.Services.Implements
{
    public class PostPaymentProcessingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PostPaymentProcessingService> _logger;

        public PostPaymentProcessingService(IServiceProvider serviceProvider, ILogger<PostPaymentProcessingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessOrderAsync(string orderId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var merchantRepository = scope.ServiceProvider.GetRequiredService<IMerchantRepository>();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                try
                {
                    var order = await orderRepository.GetOrderById(orderId);
                    if (order == null) return;

                    // Update order status to PENDING
                    var updateOrderStatusDTO = new UpdateOrderStatusDTO
                    {
                        Status = OrderStatusEnum.PENDING.ToString(),
                        UpdatedDate = DateTime.Now
                    };
                    await orderService.UpdateOrderAsync(orderId, updateOrderStatusDTO);

                    var systemIncome = order.TotalAmount * 0.05;
                    order.SystemIncome = systemIncome;

                    var merchantIncomes = order.OrderProducts
                        .GroupBy(op => op.MerchantId)
                        .Select(g => new
                        {
                            MerchantId = g.Key,
                            Income = g.Sum(op => op.Price * op.Quantity) * 0.95
                        })
                        .ToList();

                    foreach (var merchantIncome in merchantIncomes)
                    {
                        var merchant = await merchantRepository.GetMerchantByIdAsync(merchantIncome.MerchantId);
                        if (merchant != null)
                        {
                            merchant.OrderIncomes.Add(new OrderIncome
                            {
                                OrderId = order._id,
                                Income = merchantIncome.Income
                            });
                            await merchantRepository.UpdateMerchantAsync(merchant);
                        }
                    }

                    // Update order status to PROCESSING
                    updateOrderStatusDTO = new UpdateOrderStatusDTO
                    {
                        Status = OrderStatusEnum.PROCESSING.ToString(),
                        UpdatedDate = DateTime.Now
                    };
                    await orderService.UpdateOrderAsync(orderId, updateOrderStatusDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing order.");
                }
            }
        }
    }
}
