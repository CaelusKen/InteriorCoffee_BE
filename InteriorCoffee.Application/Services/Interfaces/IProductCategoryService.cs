using InteriorCoffee.Application.DTOs.ProductCategory;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategory>> GetProductCategoryListAsync();
        Task<ProductCategory> GetProductCategoryByIdAsync(string id);
        Task CreateProductCategoryAsync(CreateProductCategoryDTO productCategory);
        Task UpdateProductCategoryAsync(string id, UpdateProductCategoryDTO productCategory);
        Task DeleteProductCategoryAsync(string id);
    }
}
