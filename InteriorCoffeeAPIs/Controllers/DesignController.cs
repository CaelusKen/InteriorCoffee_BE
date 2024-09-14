using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Design;
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
    public class DesignController : BaseController<DesignController>
    {
        private readonly IDesignService _designService;

        public DesignController(ILogger<DesignController> logger, IDesignService designService) : base(logger)
        {
            _designService = designService;
        }

        [HttpGet(ApiEndPointConstant.Design.DesignsEndpoint)]
        [ProducesResponseType(typeof(List<Design>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all designs with pagination")]
        public async Task<IActionResult> GetDesigns([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (designs, currentPage, currentPageSize, totalItems, totalPages) = await _designService.GetDesignsAsync(pageNo, pageSize);

            var response = new
            {
                PageNo = currentPage,
                PageSize = currentPageSize,
                ListSize = totalItems,
                CurrentPageSize = designs.Count,
                TotalPage = totalPages,
                Designs = designs
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Design.DesignEndpoint)]
        [ProducesResponseType(typeof(Design), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a design by id")]
        public async Task<IActionResult> GetDesignById(string id)
        {
            var result = await _designService.GetDesignByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
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
            if (existingDesign == null)
            {
                return NotFound();
            }

            await _designService.UpdateDesignAsync(id, updateDesign);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Design.DesignEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a design")]
        public async Task<IActionResult> DeleteDesign(string id)
        {
            var design = await _designService.GetDesignByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            await _designService.DeleteDesignAsync(id);
            return Ok("Action success");
        }
    }
}
