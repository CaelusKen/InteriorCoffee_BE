using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.PaymentModel.PayPal
{
    public class PaypalRequestModel
    {
        public double Amount { get; set; }
        public string Currency {  get; set; }
        public string OrderId { get; set; }
    }
}
