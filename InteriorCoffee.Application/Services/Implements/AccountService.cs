using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAccountList();
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Account ID cannot be null or empty.");

            return await _accountRepository.GetAccountById(id);
        }

        public async Task CreateAccountAsync(Account account)
        {
            if (account == null) throw new ArgumentException("Account cannot be null.");

            await _accountRepository.CreateAccount(account);
        }

        public async Task UpdateAccountAsync(string id, Account account)
        {
            if (string.IsNullOrEmpty(id) || account == null) throw new ArgumentException("Account ID and data cannot be null or empty.");

            await _accountRepository.UpdateAccount(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Account ID cannot be null or empty.");
            await _accountRepository.DeleteAccount(id);
        }
    }
}
