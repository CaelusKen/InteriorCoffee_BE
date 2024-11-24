using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Template;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class TemplateController : BaseController<TemplateController>
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ILogger<TemplateController> logger, ITemplateService templateService) : base(logger)
        {
            _templateService = templateService;
        }

        [HttpGet(ApiEndPointConstant.Template.TemplatesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Template>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get all templates with pagination")]
        public async Task<IActionResult> GetTemplates([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            try
            {
                var (templates, currentPage, currentPageSize, totalItems, totalPages) = await _templateService.GetTemplatesAsync(pageNo, pageSize);

                var response = new Paginate<Template>
                {
                    Items = templates,
                    PageNo = currentPage,
                    PageSize = currentPageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                };

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

        [HttpGet(ApiEndPointConstant.Template.TemplateEndpoint)]
        [ProducesResponseType(typeof(GetTemplateDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a template by id")]
        public async Task<IActionResult> GetTemplateById(string id)
        {
            var result = await _templateService.GetTemplateById(id);
            return Ok(result);
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPost(ApiEndPointConstant.Template.TemplatesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create template")]
        public async Task<IActionResult> CreateTemplate(CreateTemplateDTO template)
        {
            await _templateService.CreateTemplate(template);
            return Ok("Action success");
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPatch(ApiEndPointConstant.Template.TemplateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a template's data")]
        public async Task<IActionResult> UpdateTemplates(string id, [FromBody] UpdateTemplateDTO updateTemplate)
        {
            await _templateService.UpdateTemplate(id, updateTemplate);
            return Ok("Action success");
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpDelete(ApiEndPointConstant.Template.TemplateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a template")]
        public async Task<IActionResult> DeleteTemplates(string id)
        {
            await _templateService.DeleteTemplate(id);
            return Ok("Action success");
        }
    }
}
