using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Merchant
{
    public class CreateMerchantDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string LogoUrl { get; set; }
        public string? Description { get; set; }
        //public string Status { get; set; }
        public string MerchantCode { get; set; } = null!;
        public string PolicyDocument { get; set; }
        public string Website { get; set; }
    }
}

