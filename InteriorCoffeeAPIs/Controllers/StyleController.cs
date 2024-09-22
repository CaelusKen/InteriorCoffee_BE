using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Style;
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
    public class StyleController : BaseController<StyleController>
    {
        private readonly IStyleService _styleService;

        public StyleController(ILogger<StyleController> logger, IStyleService styleService) : base(logger)
        {
            _styleService = styleService;
        }

        [HttpGet(ApiEndPointConstant.Style.StylesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Style>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all styles with pagination")]
        public async Task<IActionResult> GetStyles([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (styles, currentPage, currentPageSize, totalItems, totalPages) = await _styleService.GetStylesAsync(pageNo, pageSize);

            var response = new Paginate<Style>
            {
                Items = styles,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalPages = totalPages,
                TotalItems = styles.Count,
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Style.StyleEndpoint)]
        [ProducesResponseType(typeof(Style), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a style by id")]
        public async Task<IActionResult> GetStyleById(string id)
        {
            var result = await _styleService.GetStyleById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Style.StylesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create style")]
        public async Task<IActionResult> CreateStyle(StyleDTO style)
        {
            await _styleService.CreateStyle(style);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Style.StyleEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a style's data")]
        public async Task<IActionResult> UpdateStyles(string id, [FromBody] StyleDTO updateStyle)
        {
            await _styleService.UpdateStyle(id, updateStyle);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Style.StyleEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a style")]
        public async Task<IActionResult> DeleteStyles(string id)
        {
            await _styleService.DeleteStyle(id);
            return Ok("Action success");
        }
    }
}
