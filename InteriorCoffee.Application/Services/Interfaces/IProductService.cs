using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> GetProductsAsync(
            int? pageNo, int? pageSize, decimal? minPrice, decimal? maxPrice, OrderBy orderBy, ProductFilter filter, string keyword = null);
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(CreateProductDTO product);
        Task UpdateProductAsync(string id, UpdateProductDTO product);
        Task SoftDeleteProductAsync(string id);
        Task DeleteProductAsync(string id);
    }
}
