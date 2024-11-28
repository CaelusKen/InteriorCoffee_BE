using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Order
{
    public class CreateOrderDTO
    {
        //public DateTime OrderDate { get; set; }
        //public string Status { get; set; }
        public double VAT { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderProducts> OrderProducts { get; set; } = null!;
        public string VoucherId { get; set; } 
        public string AccountId { get; set; } = null!;
        //public DateTime CreatedDate { get; set; }
    }
}
