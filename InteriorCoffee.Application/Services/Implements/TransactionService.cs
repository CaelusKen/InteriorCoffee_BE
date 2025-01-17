﻿using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Pagination;
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

        public async Task<(List<Transaction>, int, int, int, int)> GetTransactionsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            var (transactions, totalItems, currentPageSize, totalPages) = await _transactionRepository.GetTransactionsAsync(pagination.PageNo, pagination.PageSize);
            return (transactions, pagination.PageNo, currentPageSize, totalItems, totalPages);
        }

        public async Task<Transaction> GetTransactionById(string id)
        {
            var result =  await _transactionRepository.GetTransaction(
                predicate: tr => tr._id.Equals(id));

            if(result == null) throw new NotFoundException($"Transaction id {id} cannot be found");

            return result;
        }

        public async Task CreateTransaction(CreateTransactionDTO createTransaction)
        {
            Transaction transaction = _mapper.Map<Transaction>(createTransaction);
            await _transactionRepository.CreateTransaction(transaction);
        }

        public async Task UpdateTransaction(string id, UpdateTransactionDTO updateTransacrtion)
        {
            Transaction transaction = await _transactionRepository.GetTransaction(
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
            Transaction transaction = await _transactionRepository.GetTransaction(
                predicate: tr => tr._id.Equals(id));

            if (transaction == null) throw new NotFoundException($"Transaction id {id} cannot be found");

            await _transactionRepository.DeleteTransaction(id);
        }
    }
}
