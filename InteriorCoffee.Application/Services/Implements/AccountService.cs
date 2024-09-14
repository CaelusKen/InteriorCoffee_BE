using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Implements;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository, IRoleRepository roleRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task<(List<Account>, int, int, int, int)> GetAccountsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            var (accounts, totalItems, currentPageSize, totalPages) = await _accountRepository.GetAccountsAsync(pagination.PageNo, pagination.PageSize);
            return (accounts, pagination.PageNo, currentPageSize, totalItems, totalPages);
        }


        public async Task<Account> GetAccountByIdAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            return _mapper.Map<Account>(account);
        }

        public async Task CreateAccountAsync(CreateAccountDTO createAccountDTO)
        {
            // Check if an account with the given email already exists
            Account existingAccount = await _accountRepository.GetAccountByCondition(
                predicate: a => a.Email.Equals(createAccountDTO.Email));
            if (existingAccount != null) throw new ConflictException("Email has already existed");

            // Get the role for the new account
            Role customerRole = await _roleRepository.GetRoleByCondition(
                predicate: r => r.Name.Equals(AccountRoleEnum.CUSTOMER.ToString()));

            // Setup new account information
            Account newAccount = _mapper.Map<Account>(createAccountDTO);
            newAccount.RoleId = customerRole._id;

            // Create new account
            await _accountRepository.CreateAccount(newAccount);
        }


        public async Task UpdateAccountAsync(string id, UpdateAccountDTO updateAccountDTO)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            _mapper.Map(updateAccountDTO, account);
            await _accountRepository.UpdateAccount(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {id} not found.");
            }
            await _accountRepository.DeleteAccount(id);
        }
    }
}
