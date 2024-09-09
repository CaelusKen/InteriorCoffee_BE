using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.ProductCategory;
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
    public class ProductCategoryController : BaseController<ProductCategoryController>
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(ILogger<ProductCategoryController> logger, IProductCategoryService productCategoryService) : base(logger)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet(ApiEndPointConstant.ProductCategory.ProductCategoriesEndpoint)]
        [ProducesResponseType(typeof(List<ProductCategory>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all product categories")]
        public async Task<IActionResult> GetAllProductCategories()
        {
            var result = await _productCategoryService.GetProductCategoryListAsync();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(ProductCategory), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a product category by id")]
        public async Task<IActionResult> GetProductCategoryById(string id)
        {
            var result = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.ProductCategory.ProductCategoriesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create product category")]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryDTO productCategory)
        {
            await _productCategoryService.CreateProductCategoryAsync(productCategory);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a product category's data")]
        public async Task<IActionResult> UpdateProductCategory(string id, [FromBody] UpdateProductCategoryDTO updateProductCategory)
        {
            var existingProductCategory = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (existingProductCategory == null)
            {
                return NotFound();
            }

            await _productCategoryService.UpdateProductCategoryAsync(id, updateProductCategory);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a product category")]
        public async Task<IActionResult> DeleteProductCategory(string id)
        {
            var productCategory = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            await _productCategoryService.DeleteProductCategoryAsync(id);
            return Ok("Action success");
        }
    }
}
