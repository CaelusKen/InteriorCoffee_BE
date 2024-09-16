using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Helpers;
using InteriorCoffee.Domain.PaymentModel.PayPal;
using InteriorCoffee.Domain.PaymentModel.VNPay;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> CreatePaymentUrl(HttpContext context, CreateTransactionDTO model);
        public Task<VnPaymentResponseModel> PaymentExecute(VnPayReturnResponseModel collections);

        public Task<CreateOrderResponse> CreatePaypalOrder(CreateTransactionDTO model);
        public Task<CaptureOrderResponse> CapturePaypalOrder(string orderId);
    }
}
