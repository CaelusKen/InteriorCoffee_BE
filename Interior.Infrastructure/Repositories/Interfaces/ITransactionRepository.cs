using InteriorCoffee.Domain.Models;
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
        Task<List<Transaction>> GetTransactionList();
        Task<Transaction> GetTransactionById(string id);
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(string id);

        public Task<List<Transaction>> GetTransactionListByCondition(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null);
        public Task<Transaction> GetTransactionByCondition(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null);
    }
}
