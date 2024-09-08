using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DesignController : ControllerBase
    {
        private readonly IDesignService _designService;

        public DesignController(IDesignService designService)
        {
            _designService = designService;
        }

        // GET: api/Design
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Design>>> GetDesigns()
        {
            var designs = await _designService.GetAllDesignsAsync();
            return Ok(designs);
        }

        // GET: api/Design/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Design>> GetDesign(string id)
        {
            var design = await _designService.GetDesignByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }
            return Ok(design);
        }

        // POST: api/Design
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Design>> CreateDesign([FromBody] Design design)
        {
            // Ensure the _id is not set
            design._id = null;

            await _designService.CreateDesignAsync(design);
            return CreatedAtAction(nameof(GetDesign), new { id = design._id }, design);
        }

        // PUT: api/Design/{id}
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateDesign(string id, [FromBody] Design design)
        {
            if (id != design._id)
            {
                return BadRequest();
            }

            var existingDesign = await _designService.GetDesignByIdAsync(id);
            if (existingDesign == null)
            {
                return NotFound();
            }

            await _designService.UpdateDesignAsync(id, design);
            return NoContent();
        }

        // DELETE: api/Design/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesign(string id)
        {
            var design = await _designService.GetDesignByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            await _designService.DeleteDesignAsync(id);
            return NoContent();
        }
    }
}
