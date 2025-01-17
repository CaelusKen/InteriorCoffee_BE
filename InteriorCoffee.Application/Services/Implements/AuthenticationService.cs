﻿using AutoMapper;
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
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using static InteriorCoffee.Application.Constants.ApiEndPointConstant;
using Account = InteriorCoffee.Domain.Models.Account;

namespace InteriorCoffee.Application.Services.Implements
{
    public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly FirebaseService _firebaseService;

        public AuthenticationService(ILogger<AuthenticationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository,
            IMerchantRepository merchantRepository, FirebaseService firebaseService)
            : base(logger, mapper, httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _merchantRepository = merchantRepository;
            _firebaseService = firebaseService;
        }

        public async Task<AuthenticationResponseDTO> Login(LoginDTO loginDTO)
        {
            var hashPass = HashUtil.ToSHA256Hash(loginDTO.Password);

            Account account = await _accountRepository.GetAccount(
                predicate: a => a.Email.Equals(loginDTO.Email) && a.Password.Equals(hashPass));
            if (account == null) throw new UnauthorizedAccessException("Incorrect email or password");

            if (!account.Status.Equals(AccountStatusEnum.ACTIVE.ToString())) throw new UnauthorizedAccessException("Your account is currently not active");

            var token = JwtUtil.GenerateJwtToken(account, account.Role);
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, account.UserName, account.Email, account.Status);

            return authenticationResponse;
        }

        public async Task<AuthenticationResponseDTO> GoogleLogin(string email)
        {
            var user = await _firebaseService.GetUserByEmailAsync(email);
            if (user == null) throw new NotFoundException("Requested email not found");

            Account account = await _accountRepository.GetAccount(predicate: a => a.Email.Equals(email));
            if(account == null)
            {
                //Create new account
                RegisteredDTO registeredDTO = new RegisteredDTO()
                {
                    Email = email,
                    UserName = email.Substring(0, email.IndexOf('@')),
                    Address = string.Empty,
                    Avatar = string.Empty,
                    Password = HashUtil.ToSHA256Hash(email),
                    PhoneNumber = string.Empty
                };  

                account = _mapper.Map<Account>(registeredDTO);
                account.Role = AccountRoleEnum.CUSTOMER.ToString();

                await _accountRepository.CreateAccount(account);
            }

            var token = JwtUtil.GenerateJwtToken(account, account.Role);
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, account.UserName, account.Email, account.Status);
            return authenticationResponse;
        }
        public async Task<AuthenticationResponseDTO> Register(RegisteredDTO registeredDTO)
        {
            Account account = await _accountRepository.GetAccount(
                predicate: a => a.Email.Equals(registeredDTO.Email));
            if (account != null) throw new ConflictException("Email has already existed");

            //Setup new account information
            Account newAccount = _mapper.Map<Account>(registeredDTO);
            newAccount.Role = AccountRoleEnum.CUSTOMER.ToString();
            newAccount.Password = HashUtil.ToSHA256Hash(newAccount.Password);
            //Create new account
            await _accountRepository.CreateAccount(newAccount);

            var token = JwtUtil.GenerateJwtToken(newAccount, AccountRoleEnum.CUSTOMER.ToString());
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, newAccount.UserName, newAccount.Email, newAccount.Status);

            return authenticationResponse;
        }

        public async Task MerchantRegister(MerchantRegisteredDTO merchantRegisteredDTO)
        {
            //Create Merchant

            Domain.Models.Merchant merchant = _mapper.Map<Domain.Models.Merchant>(merchantRegisteredDTO);
            await _merchantRepository.CreateMerchant(merchant);

            Account account = await _accountRepository.GetAccount(
                predicate: a => a.Email.Equals(merchantRegisteredDTO.Email));
            if (account != null) throw new ConflictException("Email has already existed");

            //Setup new account information
            Account newAccount = _mapper.Map<Account>(merchantRegisteredDTO);
            newAccount.Role = AccountRoleEnum.MERCHANT.ToString();
            newAccount.MerchantId = merchant._id;
            newAccount.Password = HashUtil.ToSHA256Hash(newAccount.Password);

            //Create new account
            await _accountRepository.CreateAccount(newAccount);

            var token = JwtUtil.GenerateJwtToken(newAccount, AccountRoleEnum.MERCHANT.ToString());
            AuthenticationResponseDTO authenticationResponse = new AuthenticationResponseDTO(token, newAccount.UserName, newAccount.Email, newAccount.Status);
        }

        public async Task SendForgetPasswordEmail(string customerEmail)
        {
            string fromMail = "appsp377@gmail.com";
            string fromPass = "ebgy tbji verm kiyk";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test";
            message.To.Add(new MailAddress(customerEmail));
            message.Body = "<html><body> mèo ú meo, mèo ú meo </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPass),
                EnableSsl = true
            };

            smtpClient.Send(message);   
        }
    }
}
