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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        // GET: api/ProductCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            var productCategories = await _productCategoryService.GetAllProductCategoriesAsync();
            return Ok(productCategories);
        }

        // GET: api/ProductCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(string id)
        {
            var productCategory = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return Ok(productCategory);
        }

        // POST: api/ProductCategory
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<ProductCategory>> CreateProductCategory([FromBody] ProductCategory productCategory)
        {
            // Ensure the _id is not set
            productCategory._id = null;

            await _productCategoryService.CreateProductCategoryAsync(productCategory);
            return CreatedAtAction(nameof(GetProductCategory), new { id = productCategory._id }, productCategory);
        }

        // PUT: api/ProductCategory/{id}
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateProductCategory(string id, [FromBody] ProductCategory productCategory)
        {
            if (id != productCategory._id)
            {
                return BadRequest();
            }

            var existingProductCategory = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (existingProductCategory == null)
            {
                return NotFound();
            }

            await _productCategoryService.UpdateProductCategoryAsync(id, productCategory);
            return NoContent();
        }

        // DELETE: api/ProductCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(string id)
        {
            var productCategory = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            await _productCategoryService.DeleteProductCategoryAsync(id);
            return NoContent();
        }
    }
}
