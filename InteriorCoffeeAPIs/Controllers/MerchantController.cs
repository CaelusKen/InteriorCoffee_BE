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
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        // GET: api/Merchant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Merchant>>> GetMerchants()
        {
            var merchants = await _merchantService.GetAllMerchantsAsync();
            return Ok(merchants);
        }

        // GET: api/Merchant/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> GetMerchant(string id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return Ok(merchant);
        }

        // POST: api/Merchant
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Merchant>> CreateMerchant([FromBody] Merchant merchant)
        {
            // Ensure the _id is not set
            merchant._id = null;

            await _merchantService.CreateMerchantAsync(merchant);
            return CreatedAtAction(nameof(GetMerchant), new { id = merchant._id }, merchant);
        }

        // PUT: api/Merchant/{id}
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateMerchant(string id, [FromBody] Merchant merchant)
        {
            if (id != merchant._id)
            {
                return BadRequest();
            }

            var existingMerchant = await _merchantService.GetMerchantByIdAsync(id);
            if (existingMerchant == null)
            {
                return NotFound();
            }

            await _merchantService.UpdateMerchantAsync(id, merchant);
            return NoContent();
        }

        // DELETE: api/Merchant/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            await _merchantService.DeleteMerchantAsync(id);
            return NoContent();
        }
    }
}
