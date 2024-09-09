using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Merchant;
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
    public class MerchantController : BaseController<MerchantController>
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(ILogger<MerchantController> logger, IMerchantService merchantService) : base(logger)
        {
            _merchantService = merchantService;
        }

        [HttpGet(ApiEndPointConstant.Merchant.MerchantsEndpoint)]
        [ProducesResponseType(typeof(List<Merchant>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all merchants")]
        public async Task<IActionResult> GetAllMerchants()
        {
            var result = await _merchantService.GetMerchantListAsync();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(Merchant), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a merchant by id")]
        public async Task<IActionResult> GetMerchantById(string id)
        {
            var result = await _merchantService.GetMerchantByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
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

        [HttpPatch(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a merchant's data")]
        public async Task<IActionResult> UpdateMerchant(string id, [FromBody] UpdateMerchantDTO updateMerchant)
        {
            var existingMerchant = await _merchantService.GetMerchantByIdAsync(id);
            if (existingMerchant == null)
            {
                return NotFound();
            }

            await _merchantService.UpdateMerchantAsync(id, updateMerchant);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Merchant.MerchantEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a merchant")]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            await _merchantService.DeleteMerchantAsync(id);
            return Ok("Action success");
        }
    }
}
