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

        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Account>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all accounts with pagination and sorting. " +
            "Ex url: GET /api/accounts?pageNo=1&pageSize=10&sortBy=username&ascending=true\r\n")]
        public async Task<IActionResult> GetAccounts([FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null)
        {
            OrderBy orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                orderBy = new OrderBy(sortBy, ascending ?? true);
            }

            var (accounts, currentPage, currentPageSize, totalItems, totalPages) = await _accountService.GetAccountsAsync(pageNo, pageSize, orderBy);

            var response = new Paginate<Account>
            {
                Items = accounts,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
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

        [HttpPatch(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an account's data")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] JsonElement updateAccount)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            var schemaFilePath = "AccountValidate"; // Use the correct key
            var validationService = _validationServices[schemaFilePath];

            // Merge existing account data with the incoming update data
            var existingAccountJson = JsonSerializer.Serialize(existingAccount, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower });
            var existingAccountElement = JsonDocument.Parse(existingAccountJson).RootElement;

            var mergedAccount = MergeJsonElements(existingAccountElement, updateAccount);

            var jsonString = JsonSerializer.Serialize(mergedAccount, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower });
            var (isValid, errors) = validationService.ValidateJson(jsonString);

            if (!isValid)
            {
                return BadRequest(new { Errors = errors });
            }

            // Map the merged account data to an UpdateAccountDTO
            var updateAccountDto = JsonSerializer.Deserialize<UpdateAccountDTO>(jsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower });

            await _accountService.UpdateAccountAsync(id, updateAccountDto);
            return Ok("Action success");
        }

        private JsonElement MergeJsonElements(JsonElement original, JsonElement update)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();

                    foreach (var property in original.EnumerateObject())
                    {
                        if (update.TryGetProperty(property.Name, out var updatedProperty))
                        {
                            writer.WritePropertyName(property.Name);
                            updatedProperty.WriteTo(writer);
                        }
                        else
                        {
                            property.WriteTo(writer);
                        }
                    }

                    foreach (var property in update.EnumerateObject())
                    {
                        if (!original.TryGetProperty(property.Name, out _))
                        {
                            writer.WritePropertyName(property.Name);
                            property.Value.WriteTo(writer);
                        }
                    }

                    writer.WriteEndObject();
                }

                stream.Position = 0;
                using (var document = JsonDocument.Parse(stream))
                {
                    return document.RootElement.Clone();
                }
            }
        }



        [HttpPut(ApiEndPointConstant.Account.SoftDeleteAccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Soft delete an account")]
        public async Task<IActionResult> SoftDeleteAccount(string id)
        {
            await _accountService.SoftDeleteAccountAsync(id);
            return Ok("Account successfully soft deleted");
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
