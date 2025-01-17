﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using InteriorCoffee.Application.Enums.Order;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models.Documents;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using InteriorCoffee.Domain.Models;

namespace InteriorCoffee.Application.Services.Implements
{
    public class PostPaymentProcessingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PostPaymentProcessingService> _logger;
        private const double CommissionRate = (double)CommissionRateEnum.FivePercent / 100; // Commission rate as a constant

        public PostPaymentProcessingService(IServiceProvider serviceProvider, ILogger<PostPaymentProcessingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessOrderAsync(string parentOrderId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var merchantRepository = scope.ServiceProvider.GetRequiredService<IMerchantRepository>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>(); // Assuming a product repository
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();

                using var session = await client.StartSessionAsync();
                session.StartTransaction();

                try
                {
                    var parentOrder = await orderRepository.GetOrderById(parentOrderId);
                    if (parentOrder == null)
                    {
                        _logger.LogError($"Order with ID {parentOrderId} not found.");
                        return;
                    }

                    // Verify initial status of Parent Order
                    if (parentOrder.Status != OrderStatusEnum.CREATED.ToString())
                    {
                        _logger.LogError($"Initial status of Parent Order is not CREATED. Current status: {parentOrder.Status}");
                        return;
                    }

                    _logger.LogInformation($"Retrieved Parent Order ID {parentOrderId} successfully.");

                    var systemIncome = parentOrder.TotalAmount * CommissionRate;
                    parentOrder.SystemIncome = systemIncome;
                    _logger.LogInformation($"Calculated systemIncome successfully for Parent Order ID {parentOrderId}.");

                    var merchantIncomes = parentOrder.OrderProducts
                        .GroupBy(op => op.MerchantId)
                        .Select(g => new
                        {
                            MerchantId = g.Key,
                            Products = g.ToList(),
                            Income = g.Sum(op => op.Price * op.Quantity) * (1 - CommissionRate)
                        })
                        .ToList();
                    _logger.LogInformation($"Calculated merchantIncome successfully for Parent Order ID {parentOrderId}.");

                    foreach (var merchantIncome in merchantIncomes)
                    {
                        // Create mapping options to pass the status
                        var options = new Action<IMappingOperationOptions<Order, Order>>(opt => opt.Items["Status"] = OrderStatusEnum.PENDING.ToString());

                        var subOrder = mapper.Map<Order, Order>(parentOrder, options);
                        subOrder._id = ObjectId.GenerateNewId().ToString();
                        subOrder.OrderProducts = merchantIncome.Products;
                        subOrder.TotalAmount = merchantIncome.Products.Sum(p => p.Price * p.Quantity);
                        subOrder.UpdatedDate = DateTime.Now;

                        await orderRepository.CreateOrder(subOrder);
                        _logger.LogInformation($"Created Sub Order for merchant {merchantIncome.MerchantId} with Sub Order ID {subOrder._id} and status {subOrder.Status}.");

                        var merchant = await merchantRepository.GetMerchantByIdAsync(merchantIncome.MerchantId);
                        if (merchant != null)
                        {
                            merchant.OrderIncomes.Add(new OrderIncome
                            {
                                OrderId = subOrder._id,
                                Income = merchantIncome.Income
                            });
                            await merchantRepository.UpdateMerchantAsync(merchantIncome.MerchantId, merchant);
                            _logger.LogInformation($"Transferred money to merchant {merchantIncome.MerchantId} wallet successfully for Sub Order ID {subOrder._id}.");
                        }
                    }

                    // Subtract product quantities
                    foreach (var product in parentOrder.OrderProducts)
                    {
                        await DecreaseProductQuantity(product._id, product.Quantity);
                        _logger.LogInformation($"Decreased quantity for product {product._id} by {product.Quantity}.");
                    }

                    await session.CommitTransactionAsync();

                    // Verify final status of Parent Order
                    var finalParentOrder = await orderRepository.GetOrderById(parentOrderId);
                    if (finalParentOrder.Status == OrderStatusEnum.CREATED.ToString())
                    {
                        _logger.LogInformation($"Final status of Parent Order {parentOrderId} is CREATED.");
                    }
                    else
                    {
                        _logger.LogError($"Final status of Parent Order {parentOrderId} is not CREATED. Current status: {finalParentOrder.Status}");
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogError(ex, "Error occurred while processing Parent Order.");
                }
            }
        }

        private async Task DecreaseProductQuantity(string productId, int quantity)
        {
            var productRepository = _serviceProvider.GetRequiredService<IProductRepository>();
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                product.Quantity -= quantity;
                await productRepository.UpdateProductAsync(productId, product);
            }
        }
    }
}
