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
    public interface IProductRepository
    {
        Task<(List<Product>, int)> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(string id, Product product);
        Task DeleteProductAsync(string id);

        #region Get Function
        Task<Product> GetProduct(Expression<Func<Product, bool>> predicate = null,
                                 Expression<Func<Product, object>> orderBy = null);
        Task<TResult> GetProduct<TResult>(Expression<Func<Product, TResult>> selector,
                                          Expression<Func<Product, bool>> predicate = null,
                                          Expression<Func<Product, object>> orderBy = null);
        Task<List<Product>> GetProductList(Expression<Func<Product, bool>> predicate = null,
                                           Expression<Func<Product, object>> orderBy = null);
        Task<List<TResult>> GetProductList<TResult>(Expression<Func<Product, TResult>> selector,
                                                    Expression<Func<Product, bool>> predicate = null,
                                                    Expression<Func<Product, object>> orderBy = null);
        Task<IPaginate<Product>> GetProductPagination(Expression<Func<Product, bool>> predicate = null,
                                                      Expression<Func<Product, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetProductPagination<TResult>(Expression<Func<Product, TResult>> selector,
                                                               Expression<Func<Product, bool>> predicate = null,
                                                               Expression<Func<Product, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
