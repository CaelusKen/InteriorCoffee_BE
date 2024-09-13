using AutoMapper;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _productRepository = productRepository;
        }

        public async Task<(List<Product>, int, int, int, int)> GetProductsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? 1,
                PageSize = pageSize ?? 12
            };

            var (products, totalItems, currentPageSize, totalPages) = await _productRepository.GetProductsAsync(pagination.PageNo, pagination.PageSize);
            return (products, pagination.PageNo, currentPageSize, totalItems, totalPages);
        }


        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }
            return product;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            await _productRepository.CreateProductAsync(product);
        }

        public async Task UpdateProductAsync(string id, UpdateProductDTO updateProductDTO)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }
            _mapper.Map(updateProductDTO, existingProduct);
            await _productRepository.UpdateProductAsync(id, existingProduct);
        }

        public async Task DeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
