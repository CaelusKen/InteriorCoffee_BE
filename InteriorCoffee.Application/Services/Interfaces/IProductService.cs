using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductListAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(CreateProductDTO product);
        Task UpdateProductAsync(string id, UpdateProductDTO product);
        Task DeleteProductAsync(string id);
    }
}
