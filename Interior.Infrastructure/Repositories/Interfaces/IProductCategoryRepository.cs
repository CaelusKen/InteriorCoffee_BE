using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<(List<ProductCategory>, int)> GetProductCategoriesAsync();
        Task<ProductCategory> GetProductCategoryById(string id);
        Task CreateProductCategory(ProductCategory productCategory);
        Task UpdateProductCategory(ProductCategory productCategory);
        Task DeleteProductCategory(string id);

        public Task<List<ProductCategory>> GetProductCategoryListByCondition(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null);
        public Task<ProductCategory> GetProductCategoryByCondition(Expression<Func<ProductCategory, bool>> predicate = null, Expression<Func<ProductCategory, object>> orderBy = null);
    }
}
