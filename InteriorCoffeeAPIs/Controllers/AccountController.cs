using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(List<Account>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all accounts with pagination")]
        public async Task<IActionResult> GetAccounts([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (accounts, currentPage, currentPageSize, totalItems, totalPages) = await _accountService.GetAccountsAsync(pageNo, pageSize);

            var response = new
            {
                PageNo = currentPage,
                PageSize = currentPageSize,
                ListSize = totalItems,
                CurrentPageSize = accounts.Count,
                TotalPage = totalPages,
                Accounts = accounts
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get an account by id")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var result = await _accountService.GetAccountByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create account")]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO account)
        {
            await _accountService.CreateAccountAsync(account);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an account's data")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] UpdateAccountDTO updateAccount)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound();
            }

            await _accountService.UpdateAccountAsync(id, updateAccount);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete an account")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            await _accountService.DeleteAccountAsync(id);
            return Ok("Action success");
        }
    }
}
