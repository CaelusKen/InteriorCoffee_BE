using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class OrderProducts
    {
        public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string MerchantId { get; set; } = null!;
    }
}
