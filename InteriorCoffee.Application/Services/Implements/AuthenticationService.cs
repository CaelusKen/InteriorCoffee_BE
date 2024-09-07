using AutoMapper;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Application.Utils;
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
    public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMerchantRepository _merchantRepository;

        public AuthenticationService(ILogger<AuthenticationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository,
            IRoleRepository roleRepository, IMerchantRepository merchantRepository) 
            : base(logger, mapper, httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _merchantRepository = merchantRepository;
        }

        public async Task<AuthenticationResponseDTO> Login(LoginDTO loginDTO)
        {
            Account account = await _accountRepository.GetAccountAsync(
                predicate: a => a.Email.Equals(loginDTO.Email) && a.Password.Equals(loginDTO.Password));
            if (account == null) throw new UnauthorizedAccessException("Incorrect email or password");
                
            Role accountRole = await _roleRepository.GetRoleById(account.RoleId);

            var token = JwtUtil.GenerateJwtToken(account, accountRole.Name);
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, account.UserName, account.Email, account.Status);

            return authenticationResponse;
        }

        public async Task<AuthenticationResponseDTO> Register(RegisteredDTO registeredDTO)
        {
            Account account = await _accountRepository.GetAccountAsync(
                predicate: a => a.Email.Equals(registeredDTO.Email));
            if (account != null) throw new ConflictException("Email has already existed");

            Role customerRole = await _roleRepository.GetRoleByName(AccountRoleEnum.CUSTOMER.ToString());

            //Setup new account information
            Account newAccount = _mapper.Map<Account>(registeredDTO);
            newAccount.RoleId = customerRole._id;

            //Create new account
            await _accountRepository.CreateAccount(newAccount);

            var token = JwtUtil.GenerateJwtToken(newAccount, customerRole.Name);
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, newAccount.UserName, newAccount.Email, newAccount.Status);

            return authenticationResponse;
        }

        public async Task<AuthenticationResponseDTO> MerchantRegister(MerchantRegisteredDTO merchantRegisteredDTO)
        {
            Merchant merchant = await _merchantRepository.GetMerchantByCode(merchantRegisteredDTO.MerchantCode);
            if (merchant == null) throw new NotFoundException("Merchant is not found");

            Account account = await _accountRepository.GetAccountAsync(
                predicate: a => a.Email.Equals(merchantRegisteredDTO.Email));
            if (account != null) throw new ConflictException("Email has already existed");

            Role customerRole = await _roleRepository.GetRoleByName(AccountRoleEnum.CUSTOMER.ToString());

            //Setup new account information
            Account newAccount = _mapper.Map<Account>(merchantRegisteredDTO);
            newAccount.RoleId = customerRole._id;
            newAccount.MerchantId = merchant._id;

            //Create new account
            await _accountRepository.CreateAccount(newAccount);

            var token = JwtUtil.GenerateJwtToken(newAccount, customerRole.Name);
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, newAccount.UserName, newAccount.Email, newAccount.Status);

            return authenticationResponse;
        }
    }
}
