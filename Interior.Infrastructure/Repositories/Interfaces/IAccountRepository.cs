using InteriorCoffee.Domain.Models;
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
        Task<List<Account>> GetAccountList();
        Task<Account> GetAccountById(string id);
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(string id);
        Task<List<Account>> GetAccountListByCondition(Expression<Func<Account, bool>> predicate = null, Expression<Func<Account, object>> orderBy = null);
        Task<Account> GetAccountByCondition(Expression<Func<Account, bool>> predicate = null, Expression<Func<Account, object>> orderBy = null);
    }
}
