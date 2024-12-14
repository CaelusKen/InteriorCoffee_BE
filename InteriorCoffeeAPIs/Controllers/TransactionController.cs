using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Helpers;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Domain.PaymentModel.PayPal;
using InteriorCoffee.Domain.PaymentModel.VNPay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class TransactionController : BaseController<TransactionController>
    {
        private readonly ITransactionService _transactionService;
        private readonly IPaymentService _paymentService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, IPaymentService paymentService) : base(logger)
        {
            _transactionService = transactionService;
            _paymentService = paymentService;
        }

        [HttpGet(ApiEndPointConstant.Transaction.TransactionsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<Transaction>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all transactions with pagination")]
        public async Task<IActionResult> GetTransactions([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (transactions, currentPage, currentPageSize, totalItems, totalPages) = await _transactionService.GetTransactionsAsync(pageNo, pageSize);

            var response = new Paginate<Transaction>
            {
                Items = transactions,
                PageNo = currentPage,
                PageSize = currentPageSize,
                TotalPages = totalPages,
                TotalItems = transactions.Count,
            };

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Transaction.TransactionEndpoint)]
        [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a transaction by id")]
        public async Task<IActionResult> GetTransactionById(string id)
        {
            var result = await _transactionService.GetTransactionById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create transaction (Maybe not use)")]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDTO transaction)
        {
            await _transactionService.CreateTransaction(transaction);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Transaction.TransactionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a transaction's data (Maybe not use)")]
        public async Task<IActionResult> UpdateTransactions(string id, [FromBody] UpdateTransactionDTO updateTransaction)
        {
            await _transactionService.UpdateTransaction(id, updateTransaction);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Transaction.TransactionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a transaction (Maybe not use)")]
        public async Task<IActionResult> DeleteTransactions(string id)
        {
            await _transactionService.DeleteTransaction(id);
            return Ok("Action success");
        }

        #region Third-Party Payment
        //VnPay Api Controllers
        #region VnPay
        [HttpGet(ApiEndPointConstant.Transaction.TransactionsVNPaymentReturnEndpoint)]
        [SwaggerOperation(Summary = "VnPay data return")]
        public async Task<IActionResult> PaymentReturn([FromQuery]VnPayReturnResponseModel model)
        {
            string successRedirectUrl = $"https://interi-coffee.vercel.app/customer/{model.vnp_TxnRef}/confirmation/success";
            string failureRedirectUrl = $"https://interi-coffee.vercel.app/customer/{model.vnp_TxnRef}/confirmation/fail";

            var result = await _paymentService.PaymentExecute(model);
            if(result.VnPayResponseCode == "00")
            {
                return Redirect(successRedirectUrl);
            }
            else
            {
                return Redirect(failureRedirectUrl);
            }   
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsVNPaymentEndpoint)]
        [ProducesResponseType(typeof(VnPaymentUrlReponseModel), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create VnPay Url")]
        public async Task<IActionResult> CreateVNPayment([FromBody] CreateTransactionDTO model)
        {
            var result = await _paymentService.CreatePaymentUrl(this.HttpContext, model);
            await _transactionService.CreateTransaction(model);
            return Ok(new VnPaymentUrlReponseModel
            {
                Url = result
            });
        }
        #endregion

        //Paypal Api Controllers
        #region Paypal
        [HttpPost(ApiEndPointConstant.Transaction.TransactionsPaypalCaptureEndpoint)]
        [ProducesResponseType(typeof(CaptureOrderResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Paypal Data Capture")]
        public async Task<IActionResult> PaypalPaymentCapture(string paypalOrderId)
        {
            var result = await _paymentService.CapturePaypalOrder(paypalOrderId);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsPaypalEndpoint)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create Paypal Order")]
        public async Task<IActionResult> CreatePaypalPayment([FromBody] CreateTransactionDTO model)
        {
            var result = await _paymentService.CreatePaypalOrder(model);
            await _transactionService.CreateTransaction(model);
            return Ok(result);
        }
        #endregion
        #endregion
    }
}
