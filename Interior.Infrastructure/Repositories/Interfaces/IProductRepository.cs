using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<(List<Product>, int, int, int)> GetProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(string id, Product product);
        Task DeleteProductAsync(string id);
    }
}
