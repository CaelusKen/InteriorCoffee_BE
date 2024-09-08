using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync();
        Task<ProductCategory> GetProductCategoryByIdAsync(string id);
        Task CreateProductCategoryAsync(ProductCategory productCategory);
        Task UpdateProductCategoryAsync(string id, ProductCategory productCategory);
        Task DeleteProductCategoryAsync(string id);
    }
}
