using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Product
{
    public class ProductFilter
    {
        public string Status { get; set; }
        public string CategoryId { get; set; }
        public string MerchantId { get; set; }
    }

}
