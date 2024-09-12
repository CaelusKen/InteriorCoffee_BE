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
        public Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        public Task<VnPaymentResponseModel> PaymentExecute(VnPayReturnResponseModel collections);
    }
}
