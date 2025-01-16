using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Merchant
{
    public class UpdateMerchantDTO
    {
        public string? Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string? Email { get; set; }
        public string? Address { get; set; }

        [RegularExpression(@"^[0-9]{10,12}$", ErrorMessage = "Incorrect phone number format")]
        public string? PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? MerchantCode { get; set; }
        public string? PolicyDocument { get; set; }
        public string? Website { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}

