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
    public interface ITransactionRepository
    {
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(string id);

        public Task<(List<Transaction>, int, int, int)> GetTransactionsAsync(int pageNumber, int pageSize);

        #region Get Function
        Task<Transaction> GetTransaction(Expression<Func<Transaction, bool>> predicate = null,
                                 Expression<Func<Transaction, object>> orderBy = null);
        Task<TResult> GetTransaction<TResult>(Expression<Func<Transaction, TResult>> selector,
                                          Expression<Func<Transaction, bool>> predicate = null,
                                          Expression<Func<Transaction, object>> orderBy = null);
        Task<List<Transaction>> GetTransactionList(Expression<Func<Transaction, bool>> predicate = null,
                                           Expression<Func<Transaction, object>> orderBy = null);
        Task<List<TResult>> GetTransactionList<TResult>(Expression<Func<Transaction, TResult>> selector,
                                                    Expression<Func<Transaction, bool>> predicate = null,
                                                    Expression<Func<Transaction, object>> orderBy = null);
        Task<IPaginate<Transaction>> GetTransactionPagination(Expression<Func<Transaction, bool>> predicate = null,
                                                      Expression<Func<Transaction, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetTransactionPagination<TResult>(Expression<Func<Transaction, TResult>> selector,
                                                               Expression<Func<Transaction, bool>> predicate = null,
                                                               Expression<Func<Transaction, object>> orderBy = null,
                                                               int page = 1, int size = 10);
        #endregion
    }
}
