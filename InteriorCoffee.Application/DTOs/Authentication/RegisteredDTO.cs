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
        public string UserName { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public string MerchantCode { get; set; }
    }
}
