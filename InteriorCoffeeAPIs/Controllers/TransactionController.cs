using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
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
        [ProducesResponseType(typeof(List<Transaction>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all transactions with pagination")]
        public async Task<IActionResult> GetTransactions([FromQuery] int? pageNo, [FromQuery] int? pageSize)
        {
            var (transactions, currentPage, currentPageSize, totalItems, totalPages) = await _transactionService.GetTransactionsAsync(pageNo, pageSize);

            var response = new
            {
                PageNo = currentPage,
                PageSize = currentPageSize,
                ListSize = totalItems,
                CurrentPageSize = transactions.Count,
                TotalPage = totalPages,
                Transactions = transactions
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
        [SwaggerOperation(Summary = "Create transaction")]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDTO transaction)
        {
            await _transactionService.CreateTransaction(transaction);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Transaction.TransactionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a transaction's data")]
        public async Task<IActionResult> UpdateTransactions(string id, [FromBody] UpdateTransacrtionDTO updateTransaction)
        {
            await _transactionService.UpdateTransaction(id, updateTransaction);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Transaction.TransactionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a transaction")]
        public async Task<IActionResult> DeleteTransactions(string id)
        {
            await _transactionService.DeleteTransaction(id);
            return Ok("Action success");
        }

        //Test purpose only
        [HttpGet(ApiEndPointConstant.Transaction.TransactionsVNPaymentReturnEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> PaymentReturn([FromQuery]VnPayReturnResponseModel model)
        {
            var result = _paymentService.PaymentExecute(model);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsVNPaymentEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> TestCreatePayment([FromBody]VnPaymentRequestModel model)
        {
            var result = _paymentService.CreatePaymentUrl(this.HttpContext, model);
            return Ok(result);
        }


        [HttpPost(ApiEndPointConstant.Transaction.TransactionsPaypalCaptureEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> PaypalPaymentCapture(string orderId)
        {
            var result = await _paymentService.CapturePaypalOrder(orderId);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsPaypalEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> TestCreatePaymentPaypal([FromBody] PaypalRequestModel model)
        {
            var result = await _paymentService.CreatePaypalOrder(model);
            return Ok(result);
        }
    }
}
