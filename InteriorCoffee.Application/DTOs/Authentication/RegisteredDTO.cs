using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Authentication
{
    public class RegisteredDTO
    {
        public string UserName { get; set; }

        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$", ErrorMessage = "Email must have character, number, special character and at least 8 letters")]
        public string Password { get; set; } = null!;

        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^[0-9]{10,12}$", ErrorMessage = "Incorrect phone number format")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }

    public class MerchantRegisteredDTO
    {
        //General information
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;

        [RegularExpression(@"^[0-9]{10,12}$", ErrorMessage = "Incorrect phone number format")]
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

        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$", ErrorMessage = "Email must have character, number, special character and at least 8 letters")]
        public string Password { get; set; } = null!;
        public string Avatar { get; set; }
    }
}
