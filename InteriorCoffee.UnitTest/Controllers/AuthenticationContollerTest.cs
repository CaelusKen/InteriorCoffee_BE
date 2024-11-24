using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Authentication;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffeeAPIs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.UnitTest.Controllers
{
    public class AuthenticationContollerTest
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthenticationController _authenticationController;

        public AuthenticationContollerTest()
        {
            logger = A.Fake<ILogger<AuthenticationController>>();
            _authenticationService = A.Fake<IAuthenticationService>();
            _authenticationController = new AuthenticationController(logger, _authenticationService);
        }

        private static LoginDTO CreateFakeLoginDto() => A.Fake<LoginDTO>();
        private static RegisteredDTO CreateFakeRegisterDto() => A.Fake<RegisteredDTO>();
        private static MerchantRegisteredDTO CreateFakeMerchantRegisterDto() => A.Fake<MerchantRegisteredDTO>();

        #region Post Function Test
        [Fact]
        public async void AuthenticationController_Login_ReturnSuccess()
        {
            //Arrange
            var login = CreateFakeLoginDto();

            //Act
            var result = (OkObjectResult)await _authenticationController.Login(login);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<AuthenticationResponseDTO>();
        }

        [Fact]
        public async void AuthenticationController_CustomerRegister_ReturnSuccess()
        {
            //Arrange
            var register = CreateFakeRegisterDto();

            //Act
            var result = (OkObjectResult)await _authenticationController.CustomerRegister(register);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<AuthenticationResponseDTO>();
        }

        [Fact]
        public async void AuthenticationController_MerchantRegister_ReturnSuccess()
        {
            //Arrange
            var register = CreateFakeMerchantRegisterDto();

            //Act
            var result = (OkObjectResult)await _authenticationController.MerchantRegister(register);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async void AuthenticationController_SendForgetPasswordEmail_ReturnSuccess()
        {
            //Arrange
            var email = "phunguyen2003@gmail.com";

            //Act
            var result = (OkObjectResult)await _authenticationController.SendForgetPasswordEmail(email);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
