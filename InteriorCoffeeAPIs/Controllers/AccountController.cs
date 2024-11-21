using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Threading.Tasks;
using System.Text.Json;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Application.Enums.Account;
using Microsoft.AspNetCore.Authorization;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        private readonly IDictionary<string, JsonValidationService> _validationServices;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IDictionary<string, JsonValidationService> validationServices) : base(logger)
        {
            _accountService = accountService;
            _validationServices = validationServices;
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(AccountResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get all accounts with pagination, sorting, and filtering. " +
            "Ex url: GET /api/accounts?pageNo=1&pageSize=10&sortBy=username&ascending=true&roleId=123&status=active&keyword=john\r\n")]
        public async Task<IActionResult> GetAccounts([FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null,
                                                     [FromQuery] string role = null, [FromQuery] string status = null, [FromQuery] string keyword = null)
        {
            try
            {
                OrderBy orderBy = null;
                if (!string.IsNullOrEmpty(sortBy))
                {
                    orderBy = new OrderBy(sortBy, ascending ?? true);
                }

                var filter = new AccountFilterDTO
                {
                    Status = status,
                    Role = role
                };

                var response = await _accountService.GetAccountsAsync(pageNo, pageSize, orderBy, filter, keyword);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT, AccountRoleEnum.CUSTOMER)]
        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get an account by id")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var result = await _accountService.GetAccountByIdAsync(id);
            return Ok(result);
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpGet(ApiEndPointConstant.Account.AccountsEmailEndpoint)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get an account by email")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            var result = await _accountService.GetAccountByEmail(email);
            return Ok(result);
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPost(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create account for manager and consultant")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDTO account)
        {
            var schemaFilePath = "AccountValidate"; // Use the correct key
            var validationService = _validationServices[schemaFilePath];
            var jsonString = JsonSerializer.Serialize(account, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower });
            var (isValid, errors) = validationService.ValidateJson(jsonString);

            if (!isValid)
            {
                return BadRequest(new { Errors = errors });
            }

            await _accountService.CreateAccountAsync(account);
            return Ok("Action success");
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT, AccountRoleEnum.CUSTOMER)]
        [HttpPatch(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an account's data")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] JsonElement updateAccount)
        {
            try
            {
                await _accountService.UpdateAccountAsync(id, updateAccount);
                _logger.LogInformation("Account updated successfully with id {id}", id);
                return Ok("Action success");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }


        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT, AccountRoleEnum.CUSTOMER)]
        [HttpPatch(ApiEndPointConstant.Account.SoftDeleteAccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Soft delete an account")]
        [Consumes("application/json")]
        public async Task<IActionResult> SoftDeleteAccount(string id, [FromBody] JsonElement? requestBody = null)
        {
            if (requestBody == null || !requestBody.HasValue || requestBody.Value.ValueKind == JsonValueKind.Undefined)
            {
                using (var doc = JsonDocument.Parse("{}"))
                {
                    requestBody = doc.RootElement.Clone();
                }
            }

            try
            {
                await _accountService.SoftDeleteAccountAsync(id);
                return Ok("Account successfully soft deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while soft deleting account");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER)]
        [HttpDelete(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete an account")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Ok("Account successfully deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting account");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
