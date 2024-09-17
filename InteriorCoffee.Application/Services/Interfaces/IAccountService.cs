using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(List<Account>, int, int, int, int)> GetAccountsAsync(int? pageNo, int? pageSize, OrderBy orderBy);
        Task<Account> GetAccountByIdAsync(string id);
        Task CreateAccountAsync(CreateAccountDTO createAccountDTO);
        Task UpdateAccountAsync(string id, UpdateAccountDTO account);
        Task SoftDeleteAccountAsync(string id);
        Task DeleteAccountAsync(string id);
    }
}
