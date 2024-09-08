using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        public string PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public string Currency { get; set; }

        public string AccountId { get; set; }
        public string OrderId { get; set; }
    }
}
