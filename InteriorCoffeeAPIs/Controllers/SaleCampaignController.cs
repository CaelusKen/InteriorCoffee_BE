using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.SaleCampaign;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class SaleCampaignController : BaseController<SaleCampaignController>
    {
        private readonly ISaleCampaignService _saleCampaignService;

        public SaleCampaignController(ILogger<SaleCampaignController> logger,ISaleCampaignService saleCampaignService) : base(logger)
        {
            _saleCampaignService = saleCampaignService;
        }

        [HttpGet(ApiEndPointConstant.SaleCampaign.SaleCampaignsEndpoint)]
        [ProducesResponseType(typeof(List<SaleCampaign>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all sale campaigns")]
        public async Task<IActionResult> GetAllSaleCampaigns()
        {
            var result = await _saleCampaignService.GetAllCampaigns();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.SaleCampaign.SaleCampaignEndpoint)]
        [ProducesResponseType(typeof(SaleCampaign), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a sale campaign by id")]
        public async Task<IActionResult> GetSaleCampaignById(string id)
        {
            var result = await _saleCampaignService.GetCampaignById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.SaleCampaign.SaleCampaignsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create sale campaign")]
        public async Task<IActionResult> CreateSaleCampaign(CreateSaleCampaignDTO saleCampaign)
        {
            await _saleCampaignService.CreateCampaign(saleCampaign);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.SaleCampaign.SaleCampaignEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a sale campaign's data")]
        public async Task<IActionResult> UpdateSaleCampaigns(string id, [FromBody] UpdateSaleCampaignDTO updateSaleCampaign)
        {
            await _saleCampaignService.UpdateCampaign(id, updateSaleCampaign);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.SaleCampaign.SaleCampaignEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a sale campaign")]
        public async Task<IActionResult> DeleteSaleCampaigns(string id)
        {
            await _saleCampaignService.DeleteCampagin(id);
            return Ok("Action success");
        }
    }
}
