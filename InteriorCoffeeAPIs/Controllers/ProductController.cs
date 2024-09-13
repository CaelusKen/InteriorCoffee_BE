using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Product;
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
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        [HttpGet(ApiEndPointConstant.Product.ProductsEndpoint)]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all products with pagination")]
        public async Task<IActionResult> GetProducts([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (products, currentPage, currentPageSize, totalItems, totalPages) = await _productService.GetProductsAsync(pageNo, pageSize);

            var response = new
            {
                PageNo = currentPage,
                PageSize = currentPageSize,
                ListSize = totalItems,
                CurrentPageSize = products.Count,
                TotalPage = totalPages,
                Products = products
            };

            return Ok(response);
        }


        [HttpGet(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a product by id")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Product.ProductsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create product")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
        {
            await _productService.CreateProductAsync(product);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a product's data")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductDTO updateProduct)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.UpdateProductAsync(id, updateProduct);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a product")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);
            return Ok("Action success");
        }
    }
}
