using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Transaction
{
    public class UpdateTransactionDTO
    {
        public string PaymentMethod { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
    }
}
