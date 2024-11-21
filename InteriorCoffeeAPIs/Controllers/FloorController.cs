using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Swashbuckle.AspNetCore.Annotations;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Application.Constants;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class FloorController : BaseController<FloorController>
    {
        private readonly IFloorService _floorService;
        private readonly ILogger<FloorController> _logger;

        public FloorController(IFloorService floorService, ILogger<FloorController> logger) : base(logger)
        {
            _floorService = floorService;
            _logger = logger;
        }

        [HttpGet(ApiEndPointConstant.Floor.FloorEndpoint)]
        [ProducesResponseType(typeof(Floor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get a floor by ID")]
        public async Task<IActionResult> GetFloorById(string id)
        {
            try
            {
                var floor = await _floorService.GetFloorByIdAsync(id);
                return Ok(floor);
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

        [HttpPost(ApiEndPointConstant.Floor.FloorsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Create a new floor")]
        public async Task<IActionResult> CreateFloor([FromBody] CreateFloorDTO createFloorDTO)
        {
            try
            {
                var floorId = await _floorService.CreateFloorAsync(createFloorDTO);
                return Ok(floorId);
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


        [HttpPatch(ApiEndPointConstant.Floor.FloorEndpoint)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Update an existing floor")]
        public async Task<IActionResult> UpdateFloor(string id, [FromBody] JsonElement updateFloor)
        {
            try
            {
                await _floorService.UpdateFloorAsync(id, updateFloor);
                _logger.LogInformation("Floor updated successfully with id {id}", id);
                return Ok("Floor updated successfully");
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

        [HttpDelete(ApiEndPointConstant.Floor.FloorEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Delete a floor by ID")]
        public async Task<IActionResult> DeleteFloor(string id)
        {
            try
            {
                await _floorService.DeleteFloorAsync(id);
                return Ok("Floor deleted successfully");
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
    }
}
