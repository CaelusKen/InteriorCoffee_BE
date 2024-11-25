using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Design;
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
    public class DesignController : BaseController<DesignController>
    {
        private readonly IDesignService _designService;

        public DesignController(ILogger<DesignController> logger, IDesignService designService) : base(logger)
        {
            _designService = designService;
        }

        [HttpGet(ApiEndPointConstant.Design.DesignsEndpoint)]
        [ProducesResponseType(typeof(DesignResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get all designs with pagination, sorting, and filtering.")]
        public async Task<IActionResult> GetDesigns([FromQuery] int? pageNo, [FromQuery] int? pageSize, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null,
                                                    [FromQuery] string status = null, [FromQuery] string type = null, [FromQuery] List<string> categories = null, [FromQuery] string keyword = null)
        {
            //try
            //{
                OrderBy orderBy = null;
                if (!string.IsNullOrEmpty(sortBy))
                {
                    orderBy = new OrderBy(sortBy, ascending ?? true);
                }

                var filter = new DesignFilterDTO
                {
                    Status = status,
                    Type = type,
                    Categories = categories
                };

                var response = await _designService.GetDesignsAsync(pageNo, pageSize, orderBy, filter, keyword);

                return Ok(response);
            //}
            //catch (ArgumentException ex)
            //{
            //    _logger.LogError(ex, "Invalid argument provided.");
            //    return BadRequest(new { message = ex.Message });
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An error occurred while processing your request.");
            //    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            //}
        }

        [HttpGet(ApiEndPointConstant.Design.DesignEndpoint)]
        [ProducesResponseType(typeof(GetDesignDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a design by id")]
        public async Task<IActionResult> GetDesignById(string id)
        {
            var result = await _designService.GetDesignByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Design.DesignsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create design")]
        public async Task<IActionResult> CreateDesign(CreateDesignDTO design)
        {
            await _designService.CreateDesignAsync(design);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Design.DesignEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a design's data")]
        public async Task<IActionResult> UpdateDesign(string id, [FromBody] UpdateDesignDTO updateDesign)
        {
            var existingDesign = await _designService.GetDesignByIdAsync(id);

            await _designService.UpdateDesignAsync(id, updateDesign);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Design.DesignEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a design")]
        public async Task<IActionResult> DeleteDesign(string id)
        {
            var design = await _designService.GetDesignByIdAsync(id);

            await _designService.DeleteDesignAsync(id);
            return Ok("Action success");
        }
    }
}
