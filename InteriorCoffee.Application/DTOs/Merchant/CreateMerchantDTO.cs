using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Merchant
{
    public class CreateMerchantDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string MerchantCode { get; set; }
        public string PolicyDocument { get; set; }
        public string Website { get; set; }
    }
}

