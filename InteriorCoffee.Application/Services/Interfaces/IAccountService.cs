using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponseDTO> GetAccountsAsync(int? pageNo, int? pageSize, OrderBy orderBy, AccountFilterDTO filter, string keyword);
        Task<Account> GetAccountByIdAsync(string id);
        Task CreateAccountAsync(CreateAccountDTO createAccountDTO);
        Task UpdateAccountAsync(string id, JsonElement updateAccount);
        Task SoftDeleteAccountAsync(string id);
        Task DeleteAccountAsync(string id);

        Task<Account> GetAccountByEmail(string email);
    }
}
