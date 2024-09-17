using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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

        #region Get Function
        Task<ProductCategory> GetProductCategory(Expression<Func<ProductCategory, bool>> predicate = null,
                                 Expression<Func<ProductCategory, object>> orderBy = null);
        Task<TResult> GetProductCategory<TResult>(Expression<Func<ProductCategory, TResult>> selector,
                                          Expression<Func<ProductCategory, bool>> predicate = null,
                                          Expression<Func<ProductCategory, object>> orderBy = null);
        Task<List<ProductCategory>> GetProductCategoryList(Expression<Func<ProductCategory, bool>> predicate = null,
                                           Expression<Func<ProductCategory, object>> orderBy = null);
        Task<List<TResult>> GetProductCategoryList<TResult>(Expression<Func<ProductCategory, TResult>> selector,
                                                    Expression<Func<ProductCategory, bool>> predicate = null,
                                                    Expression<Func<ProductCategory, object>> orderBy = null);
        Task<IPaginate<ProductCategory>> GetProductCategoryPagination(Expression<Func<ProductCategory, bool>> predicate = null,
                                                      Expression<Func<ProductCategory, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetProductCategoryPagination<TResult>(Expression<Func<ProductCategory, TResult>> selector,
                                                               Expression<Func<ProductCategory, bool>> predicate = null,
                                                               Expression<Func<ProductCategory, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
