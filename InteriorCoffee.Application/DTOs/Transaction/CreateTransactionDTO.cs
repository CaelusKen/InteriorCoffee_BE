using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        public string AccountId { get; set; } = null!;
        public string OrderId { get; set; } = null!;

        public string FullName { get; set; } = null!;
        public string? Description { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentMethod { get; set; } = null!;
        public string? Currency { get; set; }
    }
}
