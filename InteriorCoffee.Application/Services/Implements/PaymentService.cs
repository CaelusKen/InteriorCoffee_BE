using AutoMapper;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Enums.Transaction;
using InteriorCoffee.Application.Helpers;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.PaymentModel.PayPal;
using InteriorCoffee.Domain.PaymentModel.VNPay;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hangfire;

namespace InteriorCoffee.Application.Services.Implements
{
    public class PaymentService : BaseService<PaymentService>, IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly PaypalClient _paypalClient;
        private readonly ITransactionRepository _transactionRepository;

        public PaymentService(ILogger<PaymentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, PaypalClient paypalClient, ITransactionRepository transactionRepository) : base(logger, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _paypalClient = paypalClient;
            _transactionRepository = transactionRepository;
        }

        #region VNPay
        public async Task<string> CreatePaymentUrl(HttpContext context, CreateTransactionDTO model)
        {
            var tick = DateTime.UtcNow.Ticks.ToString();
            var createDate = DateTime.UtcNow; ;

            var vnpay = new VNPayLibrary();

            vnpay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.TotalAmount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_ExpireDate", createDate.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrencyCode"]);
            vnpay.AddRequestData("vnp_IpAddr", IPAddressUtil.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "topup");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", model.OrderId);

            var paymentUrl = vnpay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<VnPaymentResponseModel> PaymentExecute(VnPayReturnResponseModel collections)
        {
            var vnpay = new VNPayLibrary();

            foreach (PropertyInfo pi in collections.GetType().GetProperties())
            {
                if (!String.IsNullOrEmpty(pi.Name) && pi.Name.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(pi.Name, pi.GetValue(collections).ToString());
                }
            }

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.vnp_SecureHash;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }

            #region Update Transaction
            InteriorCoffee.Domain.Models.Transaction transaction = await _transactionRepository.GetTransaction(
               predicate: tr => tr.OrderId.Equals(vnp_orderId));

            switch (vnp_ResponseCode)
            {
                case "00":
                    //Success
                    transaction.Status = TransactionStatusEnum.COMPLETED.ToString();
                    await _transactionRepository.UpdateTransaction(transaction);
                    // Trigger background job
                    BackgroundJob.Enqueue<PostPaymentProcessingService>(service => service.ProcessOrderAsync(transaction.OrderId)); 
                    break;
                case "24":
                    //Cancel
                    transaction.Status = TransactionStatusEnum.CANCELLED.ToString();
                    await _transactionRepository.UpdateTransaction(transaction);
                    break;
                default:
                    //Fail
                    transaction.Status = TransactionStatusEnum.FAILED.ToString();
                    await _transactionRepository.UpdateTransaction(transaction);
                    break;
            }
            #endregion

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId,
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
            };
        }
        #endregion

        #region Paypal
        public async Task<CreateOrderResponse> CreatePaypalOrder(CreateTransactionDTO model)
        {
            var response = await _paypalClient.CreateOrder(CurrencyExchangeUtil.VNDtoUSD(model.TotalAmount).ToString(), "USD", model.OrderId);
            return response;
        }

        public async Task<CaptureOrderResponse> CapturePaypalOrder(string paypalOrderId)
        {
            var response = await _paypalClient.CaptureOrder(paypalOrderId);

            var purchaseUnits = response.purchase_units;

            #region Update Transaction

            foreach (var purchaseUnit in purchaseUnits)
            {
                InteriorCoffee.Domain.Models.Transaction transaction = await _transactionRepository.GetTransaction(
                predicate: tr => tr.OrderId.Equals(purchaseUnit.reference_id));

                transaction.Status = TransactionStatusEnum.COMPLETED.ToString();
                await _transactionRepository.UpdateTransaction(transaction);

                // Trigger background job
                BackgroundJob.Enqueue<PostPaymentProcessingService>(service => service.ProcessOrderAsync(transaction.OrderId));
            }
            #endregion

            //// Trigger background job
            //var paypal_transaction = await _transactionRepository.GetTransaction(predicate: tr => tr.OrderId.Equals(paypalOrderId)); 
            //BackgroundJob.Enqueue<PostPaymentProcessingService>(service => service.ProcessOrderAsync(paypal_transaction.OrderId));

            return response;
        }
        #endregion
    }
}
