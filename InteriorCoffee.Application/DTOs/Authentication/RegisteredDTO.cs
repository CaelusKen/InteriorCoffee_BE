using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Authentication
{
    public class RegisteredDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }

    public class MerchantRegisteredDTO
    {
        //General information
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        //Merchant information
        public string MerchantName { get; set; } = null!;
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = null!;
        public string MerchantCode { get; set; } = null!;
        public string PolicyDocument { get; set; } = null!;
        public string Website { get; set; }
        
        //Account information
        public string UserName { get; set; }
        public string Password { get; set; } = null!;
        public string Avatar { get; set; }
    }
}
