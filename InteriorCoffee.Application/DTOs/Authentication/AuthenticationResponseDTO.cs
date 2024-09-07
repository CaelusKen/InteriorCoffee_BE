using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Authentication
{
    public class AuthenticationResponseDTO
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public AuthenticationResponseDTO(string accessToken, string username, string email, string status)
        {
            AccessToken = accessToken;
            Username = username;
            Email = email;
            Status = status;
        }
    }
}
