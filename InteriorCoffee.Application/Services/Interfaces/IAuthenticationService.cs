using InteriorCoffee.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponseDTO> Login(LoginDTO loginDTO);
        Task<GoogleAuthenticationResponseDTO> GoogleLogin(string email);
        Task<AuthenticationResponseDTO> Register(RegisteredDTO registeredDTO);
        Task MerchantRegister(MerchantRegisteredDTO merchantRegisteredDTO);

        Task SendForgetPasswordEmail(string customerEmail);
    }
}
