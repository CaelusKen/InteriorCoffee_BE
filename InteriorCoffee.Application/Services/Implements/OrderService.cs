﻿using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Order;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.Enums.Order;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IProductRepository productRepository) : base(logger, mapper, httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "orderdate", "OrderDate" },
            { "status", "Status" },
            { "totalamount", "TotalAmount" },
            { "orderdate", "OrderDate" }
        };

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

        public async Task<(List<Order>, int, int, int, int)> GetOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy)
        {
            try
            {
                var (allOrders, totalItems) = await _orderRepository.GetOrdersAsync();

                // Apply sorting logic only if orderBy is provided
                if (orderBy != null)
                {
                    allOrders = ApplySorting(allOrders, orderBy);
                }

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? allOrders.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)allOrders.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered orders
                var paginatedOrders = allOrders.Skip((paginationPageNo - 1) * finalPageSize)
                                               .Take(finalPageSize)
                                               .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedOrders.Count;

                return (paginatedOrders, paginationPageNo, finalPageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated orders.");
                return (new List<Order>(), 1, 0, 0, 0);
            }
        }

        public async Task<GetOrderDTO> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }

            GetOrderDTO orderDto = new GetOrderDTO()
            {
                _id = order._id,
                AccountId = order.AccountId,
                FeeAmount = order.FeeAmount,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                Status = order.Status,
                SystemIncome = order.SystemIncome,
                TotalAmount = order.TotalAmount,
                UpdatedDate = order.UpdatedDate,
                VAT = order.VAT,
                VoucherId = order.VoucherId,
            };

            List<GetOrderProductsDTO> getOrderProductsDTOs = _mapper.Map<List<GetOrderProductsDTO>>(order.OrderProducts);

            List<Product> products = await _productRepository.GetProductList();

            getOrderProductsDTOs.ForEach(async dto =>
            {
                dto.Image = products.Where(p => p._id == dto._id).FirstOrDefault().Images.Thumbnail;
            });

            orderDto.OrderProducts = getOrderProductsDTOs;

            return orderDto;
        }

        public async Task<(List<Order>, int, int, int, int)> GetMerchantOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy, string id)
        {
            try
            {
                var orderList = await _orderRepository.GetOrderList(
                    predicate: ord => ord.OrderProducts.Where(op => op.MerchantId == id).Count() == ord.OrderProducts.Count() &&
                                      ord.Status != OrderStatusEnum.CREATED.ToString());
                var totalItems = orderList.Count();

                // Apply sorting logic only if orderBy is provided
                if (orderBy != null)
                {
                    orderList = ApplySorting(orderList, orderBy);
                }

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? orderList.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)orderList.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered orders
                var paginatedOrders = orderList.Skip((paginationPageNo - 1) * finalPageSize)
                                               .Take(finalPageSize)
                                               .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedOrders.Count;

                return (paginatedOrders, paginationPageNo, finalPageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated orders.");
                return (new List<Order>(), 1, 0, 0, 0);
            }
        }

        public async Task<(List<Order>, int, int, int, int)> GetCustomerOrdersAsync(int? pageNo, int? pageSize, OrderBy orderBy, string customerId)
        {
            try
            {
                var orderList = await _orderRepository.GetOrderList(
                    predicate: ord => ord.AccountId == customerId && ord.Status != OrderStatusEnum.CREATED.ToString());
                var totalItems = orderList.Count();

                // Apply sorting logic only if orderBy is provided
                if (orderBy != null)
                {
                    orderList = ApplySorting(orderList, orderBy);
                }

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? orderList.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)orderList.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered orders
                var paginatedOrders = orderList.Skip((paginationPageNo - 1) * finalPageSize)
                                               .Take(finalPageSize)
                                               .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedOrders.Count;

                return (paginatedOrders, paginationPageNo, finalPageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated orders.");
                return (new List<Order>(), 1, 0, 0, 0);
            }
        }

        public async Task<string> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            if (createOrderDTO == null)
            {
                throw new ArgumentNullException(nameof(createOrderDTO));
            }

            var options = new Action<IMappingOperationOptions<CreateOrderDTO, Order>>(opt => opt.Items["Status"] = OrderStatusEnum.CREATED.ToString());
            var parentOrder = _mapper.Map<CreateOrderDTO, Order>(createOrderDTO, options);

            try
            {
                await _orderRepository.CreateOrder(parentOrder);
                return parentOrder._id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order.");
                throw new Exception("Failed to create order.", ex);
            }
        }

        public async Task UpdateOrderAsync(string id, UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var existingOrder = await _orderRepository.GetOrderById(id);
            if (existingOrder == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
            existingOrder.SystemIncome = updateOrderStatusDTO.SystemIncome; // Update system income
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
