using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Merchant;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class MerchantController : BaseController<MerchantController>
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(ILogger<MerchantController> logger, IMerchantService merchantService) : base(logger)
        {
            _merchantService = merchantService;
        }

        [HttpGet(ApiEndPointConstant.Merchant.MerchantsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Merchant>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all merchants with pagination and sorting. " +
            "Ex url: GET /api/merchants?pageNo=1&pageSize=10&sortBy=name&ascending=true\r\n")]
        public async Task<IActionResult> GetMerchants([FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null)
        {
            OrderBy orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                orderBy = new OrderBy(sortBy, ascending ?? true);
            }

            var (merchants, currentPage, currentPageSize, totalItems, totalPages) = await _merchantService.GetMerchantsAsync(pageNo, pageSize, orderBy);

            var response = new Paginate<Merchant>
            {
                Items = merchants,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalPages = totalPages,
                TotalItems = merchants.Count
            };

            return Ok(response);
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER)]
        [HttpGet(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(Merchant), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a merchant by id")]
        public async Task<IActionResult> GetMerchantById(string id)
        {
            var result = await _merchantService.GetMerchantByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Merchant.MerchantsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create merchant")]
        public async Task<IActionResult> CreateMerchant(CreateMerchantDTO merchant)
        {
            await _merchantService.CreateMerchantAsync(merchant);
            return Ok("Action success");
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPatch(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a merchant's data")]
        public async Task<IActionResult> UpdateMerchant(string id, [FromBody] UpdateMerchantDTO updateMerchant)
        {
            var existingMerchant = await _merchantService.GetMerchantByIdAsync(id);

            await _merchantService.UpdateMerchantAsync(id, updateMerchant);
            return Ok("Action success");
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpDelete(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a merchant")]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);

            await _merchantService.DeleteMerchantAsync(id);
            return Ok("Action success");
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPatch(ApiEndPointConstant.Merchant.MerchantVerificationEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a merchant's data")]
        public async Task<IActionResult> VerifyMerchant(string id)
        {
            var existingMerchant = await _merchantService.GetMerchantByIdAsync(id);

            await _merchantService.VerifyMerchantAsync(id);
            return Ok("Action success");
        }
    }
}
