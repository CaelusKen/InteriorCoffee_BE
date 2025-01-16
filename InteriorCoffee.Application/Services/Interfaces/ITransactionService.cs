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
        Task<(List<Transaction>, int, int, int, int)> GetTransactionsAsync(int? pageNo, int? pageSize);
        Task<Transaction> GetTransactionById(string id);
        Task CreateTransaction(CreateTransactionDTO createTransaction);
        Task UpdateTransaction(string id, UpdateTransactionDTO updateTransacrtion);
        Task DeleteTransaction(string id);
    }
}
