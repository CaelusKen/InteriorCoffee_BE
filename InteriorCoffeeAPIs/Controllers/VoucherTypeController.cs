using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.VoucherType;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class VoucherTypeController : BaseController<VoucherTypeController>
    {
        private readonly IVoucherTypeService _voucherTypeService;

        public VoucherTypeController(ILogger<VoucherTypeController> logger, IVoucherTypeService voucherTypeService) : base(logger)
        {
            _voucherTypeService = voucherTypeService;
        }

        [HttpGet(ApiEndPointConstant.VoucherType.VoucherTypesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<VoucherType>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all voucher types with pagination")]
        public async Task<IActionResult> GetVoucherTypes([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (voucherTypes, currentPage, currentPageSize, totalItems, totalPages) = await _voucherTypeService.GetVoucherTypesAsync(pageNo, pageSize);

            var response = new Paginate<VoucherType>
            {
                Items = voucherTypes,
                Page = currentPage,
                Size = currentPageSize,
                TotalPages = totalPages,
                TotalItems = voucherTypes.Count
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.VoucherType.VoucherTypeEndpoint)]
        [ProducesResponseType(typeof(VoucherType), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a voucher type by id")]
        public async Task<IActionResult> GetVoucherTypeById(string id)
        {
            var result = await _voucherTypeService.GetVoucherTypeById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.VoucherType.VoucherTypesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create voucher type")]
        public async Task<IActionResult> CreateVoucherType(VoucherTypeDTO voucherType)
        {
            await _voucherTypeService.CreateVoucherType(voucherType);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.VoucherType.VoucherTypeEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a voucher type's data")]
        public async Task<IActionResult> UpdateVoucherTypes(string id, [FromBody]VoucherTypeDTO updateVoucherType)
        {
            await _voucherTypeService.UpdateVoucherType(id, updateVoucherType);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.VoucherType.VoucherTypeEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a voucher type")]
        public async Task<IActionResult> DeleteVoucherTypes(string id)
        {
            await _voucherTypeService.DeleteVoucherType(id);
            return Ok("Action success");
        }
    }
}
