using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Account
{
    public class UpdateAccountDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string MerchantId { get; set; }
        public string Role { get; set; }
    }
}
