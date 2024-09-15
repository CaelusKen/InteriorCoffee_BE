using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<(List<Product>, int, int, int, int)> GetProductsAsync(int? pageNo, int? pageSize);
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(CreateProductDTO product);
        Task UpdateProductAsync(string id, UpdateProductDTO product);
        Task SoftDeleteProductAsync(string id);
        Task DeleteProductAsync(string id);
    }
}
