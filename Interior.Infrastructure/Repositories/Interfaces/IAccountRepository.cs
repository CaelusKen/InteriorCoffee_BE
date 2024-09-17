using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<(List<Account>, int)> GetAccountAsync();
        Task<Account> GetAccountById(string id);
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(string id);

        #region Get Function
        Task<Account> GetAccount(Expression<Func<Account, bool>> predicate = null, 
                                 Expression<Func<Account, object>> orderBy = null);
        Task<TResult> GetAccount<TResult>(Expression<Func<Account, TResult>> selector, 
                                          Expression<Func<Account, bool>> predicate = null, 
                                          Expression<Func<Account, object>> orderBy = null);
        Task<List<Account>> GetAccountList(Expression<Func<Account, bool>> predicate = null, 
                                           Expression<Func<Account, object>> orderBy = null);
        Task<List<TResult>> GetAccountList<TResult>(Expression<Func<Account, TResult>> selector, 
                                                    Expression<Func<Account, bool>> predicate = null, 
                                                    Expression<Func<Account, object>> orderBy = null);
        Task<IPaginate<Account>> GetAccountPagination(Expression<Func<Account, bool>> predicate = null,
                                                      Expression<Func<Account, object>> orderBy = null,
                                                      int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetAccountPagination<TResult>(Expression<Func<Account, TResult>> selector, 
                                                               Expression<Func<Account, bool>> predicate = null, 
                                                               Expression<Func<Account, object>> orderBy = null, 
                                                               int page = 1, int size = 10);
        #endregion
    }
}
