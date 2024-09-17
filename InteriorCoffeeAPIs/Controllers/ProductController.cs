using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
        [SwaggerOperation(Summary = "Get all products with pagination, price range, and sorting. " +
            "Ex url: GET /api/products?pageNo=1&pageSize=10&minPrice=30&maxPrice=60&sortBy=name&ascending=true\r\n")]
        public async Task<IActionResult> GetProducts([FromQuery] int? pageNo, [FromQuery] int? pageSize,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string sortBy = null, [FromQuery] bool? ascending = null,
            [FromQuery] string status = null, [FromQuery] string categoryId = null, [FromQuery] string merchantId = null)
        {
            OrderBy orderBy = null;
            if (!string.IsNullOrEmpty(sortBy))
            {
                orderBy = new OrderBy(sortBy, ascending ?? true);
            }

            var filter = new ProductFilter
            {
                Status = status,
                CategoryId = categoryId,
                MerchantId = merchantId
            };

            var (products, currentPage, currentPageSize, totalItems, filteredTotalPages, actualMinPrice, actualMaxPrice, listAfter)
                = await _productService.GetProductsAsync(pageNo, pageSize, minPrice, maxPrice, orderBy, filter);

            var response = new
            {
                PageNo = currentPage,
                PageSize = currentPageSize,
                ListSize = totalItems,
                CurrentPageSize = products.Count,
                ListAfter = listAfter,
                TotalPage = filteredTotalPages,
                MinPrice = minPrice ?? actualMinPrice,
                MaxPrice = maxPrice ?? actualMaxPrice,
                OrderBy = new
                {
                    SortBy = sortBy,
                    Ascending = ascending
                },
                Filter = new
                {
                    Status = status,
                    CategoryId = categoryId,
                    MerchantId = merchantId
                },
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

        [HttpPut(ApiEndPointConstant.Product.SoftDeleteProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Soft delete a product")]
        public async Task<IActionResult> SoftDeleteProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.SoftDeleteProductAsync(id);
            return Ok("Product successfully soft deleted");
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
