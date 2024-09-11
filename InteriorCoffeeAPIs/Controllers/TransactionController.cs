using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
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
        [SwaggerOperation(Summary = "Get all transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _transactionService.GetAllTransactions();
            return Ok(result);
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



        [HttpGet(ApiEndPointConstant.Transaction.TransactionsPaymentReturnEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> PaymentReturn(IQueryCollection collections)
        {
            var result = _paymentService.PaymentExecute(collections);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Transaction.TransactionsPaymentEndpoint)]
        [SwaggerOperation(Summary = "Test payment")]
        public async Task<IActionResult> TestCreatePayment([FromBody]VnPaymentRequestModel model)
        {
            var result = _paymentService.CreatePaymentUrl(this.HttpContext, model);
            return Ok(result);
        }
    }
}
