using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Helpers;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffee.Domain.PaymentModel.VNPay;
using InteriorCoffeeAPIs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.UnitTest.Controllers
{
    public class TransactionControllerTest
    {
        private readonly ILogger<TransactionController> logger;
        private readonly ITransactionService _transactionService;
        private readonly IPaymentService _paymentService;
        private readonly TransactionController _transactionController;

        public TransactionControllerTest()
        {
            logger = A.Fake<ILogger<TransactionController>>();
            _transactionService = A.Fake<ITransactionService>();
            _paymentService = A.Fake<IPaymentService>();
            _transactionController = new TransactionController(logger, _transactionService, _paymentService);
        }

        private static UpdateTransactionDTO CreateFakeUpdateTransactionDTO() => A.Fake<UpdateTransactionDTO>();
        private static CreateTransactionDTO CreateFakeCreateTransactionDTO() => A.Fake<CreateTransactionDTO>();
        private static VnPayReturnResponseModel CreateFakeVnPayReturnResponse() => A.Fake<VnPayReturnResponseModel>();

        #region Transaction Functions
        #region Get Function Test
        [Fact]
        public async void TransactionController_GetTransactions_ReturnTransactionList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _transactionController.GetTransactions(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<Transaction>>();
        }

        [Fact]
        public async void TransactionController_GetTransactionById_ReturnTransaction()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _transactionController.GetTransactionById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Transaction>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void TransactionController_Create_ReturnSuccess()
        {
            //Arrange
            var createTransactionDto = CreateFakeCreateTransactionDTO();

            //Act
            var result = (OkObjectResult)await _transactionController.CreateTransaction(createTransactionDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void TransactionController_Update_ReturnSuccess()
        {
            //Arrange
            var updateTransactionDto = CreateFakeUpdateTransactionDTO();

            //Act
            var result = (OkObjectResult)await _transactionController.UpdateTransactions("672d61c84e4eeed22aad9f8b", updateTransactionDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void TransactionController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _transactionController.DeleteTransactions("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
        #endregion

        #region Payment Function
        #region VNPayment
        [Fact]
        public async void TransactionController_PaymentReturn_ReturnResponse()
        {
            //Arrange
            var returnResponseModel = CreateFakeVnPayReturnResponse();


            //Act
            var result = (RedirectResult)await _transactionController.PaymentReturn(returnResponseModel);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void TransactionController_CreateVnPay_ReturnUrl()
        {
            //Arrange
            var createTransactionDTO = CreateFakeCreateTransactionDTO();


            //Act
            var result = (OkObjectResult)await _transactionController.CreateVNPayment(createTransactionDTO);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<VnPaymentUrlReponseModel>();
        }
        #endregion

        #region PayPal
        [Fact]
        public async void TransactionController_PaypalCature_ReturnCaptureResponse()
        {
            //Arrange
            var createTransactionDTO = CreateFakeCreateTransactionDTO();


            //Act
            var result = (OkObjectResult)await _transactionController.PaypalPaymentCapture("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<CaptureOrderResponse>();
        }

        [Fact]
        public async void TransactionController_CreatePaypalPayment_ReturnCreateResponse()
        {
            //Arrange
            var createTransactionDTO = CreateFakeCreateTransactionDTO();


            //Act
            var result = (OkObjectResult)await _transactionController.CreatePaypalPayment(createTransactionDTO);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<CreateOrderResponse>();
        }
        #endregion
        #endregion
    }
}
