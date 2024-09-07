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
        public Task<AuthenticationResponseDTO> Login(LoginDTO loginDTO);
        public Task<AuthenticationResponseDTO> Register(RegisteredDTO registeredDTO);
        public Task<AuthenticationResponseDTO> MerchantRegister(MerchantRegisteredDTO merchantRegisteredDTO);
    }
}
