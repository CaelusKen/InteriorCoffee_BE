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
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(string id);

        public Task<List<Transaction>> GetTransactionList(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null);
        public Task<Transaction> GetTransaction(Expression<Func<Transaction, bool>> predicate = null, Expression<Func<Transaction, object>> orderBy = null);
    }
}
