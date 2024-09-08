using AutoMapper;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class TransactionService : BaseService<TransactionService>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ILogger<TransactionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITransactionRepository transactionRepository) : base(logger, mapper, httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _transactionRepository.GetTransactionListByCondition();
        }

        public async Task<Transaction> GetTransactionById(string id)
        {
            return await _transactionRepository.GetTransactionByCondition(
                predicate: tr => tr._id.Equals(id));
        }

        public async Task CreateTransaction(CreateTransactionDTO createTransaction)
        {
            Transaction transaction = _mapper.Map<Transaction>(createTransaction);
            await _transactionRepository.CreateTransaction(transaction);
        }

        public async Task UpdateTransaction(string id, UpdateTransacrtionDTO updateTransacrtion)
        {
            Transaction transaction = await _transactionRepository.GetTransactionByCondition(
                predicate: tr => tr._id.Equals(id));

            if (transaction == null) throw new NotFoundException($"Transaction id {id} cannot be found");

            //Update transaction data
            transaction.PaymentMethod = String.IsNullOrEmpty(updateTransacrtion.PaymentMethod) ? transaction.PaymentMethod : updateTransacrtion.PaymentMethod;
            transaction.TransactionDate = updateTransacrtion.TransactionDate;
            transaction.TotalAmount = updateTransacrtion.TotalAmount;
            transaction.Currency = String.IsNullOrEmpty(updateTransacrtion.Currency) ? transaction.Currency : updateTransacrtion.Currency;
            transaction.Status = String.IsNullOrEmpty(updateTransacrtion.Status) ? transaction.Status : updateTransacrtion.Status;

            await _transactionRepository.UpdateTransaction(transaction);
        }

        public async Task DeleteTransaction(string id)
        {
            Transaction transaction = await _transactionRepository.GetTransactionByCondition(
                predicate: tr => tr._id.Equals(id));

            if (transaction == null) throw new NotFoundException($"Transaction id {id} cannot be found");

            await _transactionRepository.DeleteTransaction(id);
        }
    }
}
