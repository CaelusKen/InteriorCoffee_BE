using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        public Task<List<Transaction>> GetAllTransactions();
        public Task<Transaction> GetTransactionById(string id);
        public Task CreateTransaction(CreateTransactionDTO createTransaction);
        public Task UpdateTransaction(string id, UpdateTransacrtionDTO updateTransacrtion);
        public Task DeleteTransaction(string id);
    }
}
