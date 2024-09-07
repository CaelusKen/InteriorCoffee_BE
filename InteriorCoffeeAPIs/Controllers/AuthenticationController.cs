using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService) : base(logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(ApiEndPointConstant.Authentication.LoginEndpoint)]
        [ProducesResponseType(typeof(AuthenticationResponseDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "User Login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            var result = await _authenticationService.Login(loginDTO);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Authentication.RegisterEndpoint)]
        [ProducesResponseType(typeof(AuthenticationResponseDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Register for new customer")]
        public async Task<IActionResult> CustomerRegister([FromBody] RegisteredDTO registeredDTO)
        {
            var result = await _authenticationService.Register(registeredDTO);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Authentication.MerchantRegisterEndpoint)]
        [ProducesResponseType(typeof(AuthenticationResponseDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Register for merchant")]
        public async Task<IActionResult> MerchantRegister([FromBody] MerchantRegisteredDTO merchantRegisteredDTO)
        {
            var result = await _authenticationService.MerchantRegister(merchantRegisteredDTO);
            return Ok(result);
        }
    }
}
