using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        public string AccountId { get; set; }
        public string OrderId { get; set; }

        public string FullName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
    }
}
