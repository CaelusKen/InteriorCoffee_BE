using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.SaleCampaign;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class SaleCampaignController : BaseController<SaleCampaignController>
    {
        private readonly ISaleCampaignService _saleCampaignService;

        public SaleCampaignController(ILogger<SaleCampaignController> logger, ISaleCampaignService saleCampaignService) : base(logger)
        {
            _saleCampaignService = saleCampaignService;
        }

        [HttpGet(ApiEndPointConstant.SaleCampaign.SaleCampaignsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<SaleCampaign>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all sale campaigns with pagination")]
        public async Task<IActionResult> GetSaleCampaigns([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (saleCampaigns, currentPage, currentPageSize, totalItems, totalPages) = await _saleCampaignService.GetSaleCampaignsAsync(pageNo, pageSize);

            var response = new Paginate<SaleCampaign>
            {
                Items = saleCampaigns,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalPages = totalPages,
                TotalItems = saleCampaigns.Count
            };

            return Ok(response);
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

        //#region Campaign Products
        //[HttpPost(ApiEndPointConstant.SaleCampaign.SaleCampaignProductsEndpoint)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[SwaggerOperation(Summary = "Add products to sale campaign")]
        //public async Task<IActionResult> AddProductsToSaleCampaign(string id, [FromBody]List<string> productIds)
        //{
        //    await _saleCampaignService.AddProductsToCampaign(id, productIds);
        //    return Ok("Action success");
        //}

        //[HttpDelete(ApiEndPointConstant.SaleCampaign.SaleCampaignProductsEndpoint)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[SwaggerOperation(Summary = "Remove all products in sale campaign")]
        //public async Task<IActionResult> RemoveAllProductsInCampaign(string id)
        //{
        //    await _saleCampaignService.RemoveAllProductsInCampaign(id);
        //    return Ok("Action success");
        //}
        //#endregion
    }
}
