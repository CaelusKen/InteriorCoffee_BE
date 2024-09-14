﻿using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(List<Account>, int, int, int, int)> GetAccountsAsync(int? pageNo, int? pageSize);
        Task<Account> GetAccountByIdAsync(string id);
        Task CreateAccountAsync(CreateAccountDTO createAccountDTO);
        Task UpdateAccountAsync(string id, UpdateAccountDTO account);
        Task DeleteAccountAsync(string id);
    }
}
