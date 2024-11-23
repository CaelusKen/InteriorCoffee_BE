using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using static InteriorCoffee.Application.Constants.ApiEndPointConstant;
using JsonSerializer = System.Text.Json.JsonSerializer;
using AutoMapper;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Enums.Account;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;
        private readonly IDictionary<string, JsonValidationService> _validationServices;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IDictionary<string, JsonValidationService> validationServices, IMapper mapper) : base(logger)
        {
            _productService = productService;
            _validationServices = validationServices;
            _mapper = mapper;
        }


        #region "Schema Swagger"
        /// <summary>
        /// Get all products with pagination, price range, and sorting.
        /// </summary>
        /// <param name="pageNo">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="minPrice">Minimum price filter.</param>
        /// <param name="maxPrice">Maximum price filter.</param>
        /// <param name="sortBy">Sort by field.</param>
        /// <param name="isAscending">Sort order.</param>
        /// <param name="status">Status filter.</param>
        /// <param name="categoryId">Category ID filter.</param>
        /// <param name="merchantId">Merchant ID filter.</param>
        /// <param name="keyword">Search keyword.</param>
        /// <param name="isAvailability">Availability filter.</param>
        /// <returns>Paginated list of products.</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Product.ProductsEndpoint)]
        [ProducesResponseType(typeof(ProductResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Get all products with pagination, price range, and sorting. " +
            "Ex url: GET /api/products?pageNo=1&pageSize=10&minPrice=30&maxPrice=60&sortBy=name&isAscending=true&...(more)\r\n")]
        public async Task<IActionResult> GetProducts([FromQuery] int? pageNo, [FromQuery] int? pageSize,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string sortBy = null, [FromQuery] bool? isAscending = null,
            [FromQuery] string status = null, [FromQuery] string categoryId = null, [FromQuery] string merchantId = null, [FromQuery] string keyword = null,
            [FromQuery] bool? isAvailability = null)
        {
            try
            {
                OrderBy orderBy = null;
                if (!string.IsNullOrEmpty(sortBy))
                {
                    orderBy = new OrderBy(sortBy, isAscending ?? true);
                }

                var filter = new ProductFilterDTO
                {
                    Status = status,
                    CategoryId = categoryId,
                    MerchantId = merchantId,
                    IsAvailability = isAvailability
                };

                var response = await _productService.GetProductsAsync(pageNo, pageSize, minPrice, maxPrice, orderBy, filter, keyword);

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


        [HttpGet(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(InteriorCoffee.Domain.Models.Product), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a product by id")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return Ok(result);
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPost(ApiEndPointConstant.Product.ProductsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO product)
        {
            var schemaFilePath = "ProductValidate"; // Ensure this key is correct
            var validationService = _validationServices[schemaFilePath];
            var jsonString = JsonSerializer.Serialize(product, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower });
            var (isValid, errors) = validationService.ValidateJson(jsonString, isUpdate: false);

            if (!isValid)
            {
                _logger.LogError("Validation failed: {Errors}", errors);
                return BadRequest(new { Errors = errors });
            }

            await _productService.CreateProductAsync(product);
            return Ok(new { Message = "Product created successfully" });
        }

        //[CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPatch(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a product's data")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] JsonElement updateProduct)
        {
            try
            {
                await _productService.UpdateProductAsync(id, updateProduct);
                _logger.LogInformation("Product updated successfully with id {id}", id);
                return Ok(new { Message = "Product updated successfully" });
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


        #region "Patch update" (Clean, use Util instead)
        //public static JsonElement MergeJsonElements(JsonElement original, JsonElement update)
        // {
        //     using (var stream = new MemoryStream())
        //     {
        //         using (var writer = new Utf8JsonWriter(stream))
        //         {
        //             writer.WriteStartObject();

        //             foreach (var property in original.EnumerateObject())
        //             {
        //                 if (update.TryGetProperty(property.Name, out var updatedProperty))
        //                 {
        //                     writer.WritePropertyName(property.Name);
        //                     updatedProperty.WriteTo(writer);
        //                 }
        //                 else
        //                 {
        //                     property.WriteTo(writer);
        //                 }
        //             }

        //             foreach (var property in update.EnumerateObject())
        //             {
        //                 if (!original.TryGetProperty(property.Name, out _))
        //                 {
        //                     writer.WritePropertyName(property.Name);
        //                     property.Value.WriteTo(writer);
        //                 }
        //             }

        //             writer.WriteEndObject();
        //         }

        //         stream.Position = 0;
        //         using (var document = JsonDocument.Parse(stream))
        //         {
        //             return document.RootElement.Clone();
        //         }
        //     }
        // }
        #endregion

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpPatch(ApiEndPointConstant.Product.SoftDeleteProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Soft delete a product")]
        public async Task<IActionResult> SoftDeleteProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            await _productService.SoftDeleteProductAsync(id);
            return Ok("Product successfully soft deleted");
        }

        [CustomAuthorize(AccountRoleEnum.MANAGER, AccountRoleEnum.MERCHANT)]
        [HttpDelete(ApiEndPointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a product")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            await _productService.DeleteProductAsync(id);
            return Ok("Action success");
        }
    }
}
