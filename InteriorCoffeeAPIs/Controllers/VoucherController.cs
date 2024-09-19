using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class VoucherController : BaseController<VoucherController>
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(ILogger<VoucherController> logger, IVoucherService voucherService) : base(logger)
        {
            _voucherService = voucherService;
        }

        [HttpGet(ApiEndPointConstant.Voucher.VouchersEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Voucher>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all vouchers with pagination")]
        public async Task<IActionResult> GetVouchers([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (vouchers, currentPage, currentPageSize, totalItems, totalPages) = await _voucherService.GetVouchersAsync(pageNo, pageSize);

            var response = new Paginate<Voucher>
            {
                Items = vouchers,
                Page = currentPage,
                Size = currentPageSize,
                TotalPages = totalPages,
                TotalItems = vouchers.Count,
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Voucher.VoucherEndpoint)]
        [ProducesResponseType(typeof(Voucher), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a voucher by id")]
        public async Task<IActionResult> GetVoucherById(string id)
        {
            var result = await _voucherService.GetVoucherById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Voucher.VouchersEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create voucher")]
        public async Task<IActionResult> CreateVoucher(CreateVoucherDTO voucher)
        {
            await _voucherService.CreateVoucher(voucher);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Voucher.VoucherEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a voucher's data")]
        public async Task<IActionResult> UpdateVouchers(string id, [FromBody] UpdateVoucherDTO updateVoucher)
        {
            await _voucherService.UpdateVoucher(id, updateVoucher);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Voucher.VoucherEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a voucher")]
        public async Task<IActionResult> DeleteVouchers(string id)
        {
            await _voucherService.DeleteVoucher(id);
            return Ok("Action success");
        }
    }
}
