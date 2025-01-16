using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Account;
using InteriorCoffee.Application.DTOs.Product;
using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Controllers;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.UnitTest.Controllers
{
    public class AccountControllerTest
    {
        private readonly ILogger<AccountController> logger;
        private readonly ILogger<JsonValidationService> validationLogger;
        private readonly IAccountService _accountService;
        private readonly AccountController _accountController;
        private readonly IDictionary<string, JsonValidationService> _validationServicesDict;

        public AccountControllerTest()
        {
            validationLogger = A.Fake<ILogger<JsonValidationService>>();
            _validationServicesDict = new Dictionary<string, JsonValidationService>();
            SetupJsonValidation();

            logger = A.Fake<ILogger<AccountController>>();
            _accountService = A.Fake<IAccountService>();
            _accountController = new AccountController(logger, _accountService, _validationServicesDict);
        }

        #region Setup Fake Data
        private void SetupJsonValidation()
        {
            var schemaFilePath = GetFilePath();
            var validationService = new JsonValidationService(schemaFilePath, validationLogger);
            var schemaName = "AccountValidate";
            _validationServicesDict[schemaName] = validationService;
        }

        private string GetFilePath()
        {
            var schemaFilePath = Path.GetFullPath("AccountValidate.json");
            var removeIndex = schemaFilePath.IndexOf("bin");
            var result = schemaFilePath.Substring(0, removeIndex);
            result = result + @"ValidationFile\AccountValidate.json";

            return result;
        }

        private static CreateAccountDTO CreateFakeCreateAccountDTO() => A.Fake<CreateAccountDTO>();
        private static UpdateAccountDTO CreateFakeUpdateAccountDTO() => A.Fake<UpdateAccountDTO>();
        #endregion

        #region Get Function Test
        [Fact]
        public async void AccountController_GetAccounts_ReturnAccountList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _accountController.GetAccounts(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<AccountResponseDTO>();
        }

        [Fact]
        public async void AccountController_GetAccountById_ReturnAccount()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _accountController.GetAccountById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Account>();
        }

        [Fact]
        public async void AccountController_GetAccountByEmail_ReturnAccount()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _accountController.GetAccountByEmail("test@gmail.com");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Account>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void AccountController_Create_ReturnSuccess()
        {
            //Arrange
            var createAccountDto = CreateFakeCreateAccountDTO();
            createAccountDto.UserName = "Test";
            createAccountDto.Email = "Test@gmail.com";
            createAccountDto.PhoneNumber = "0987654321";
            createAccountDto.Password = "ab123Adsc213@Ajd";
            createAccountDto.Avatar = string.Empty;
            createAccountDto.Address = string.Empty;
            createAccountDto.MerchantId = "none";

            //Act
            var result = (OkObjectResult)await _accountController.CreateAccount(createAccountDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void AccountController_Update_ReturnSuccess()
        {
            //Arrange
            var updateAccountDto = CreateFakeUpdateAccountDTO();

            var jsonUpdatedProduct = JsonConvert.SerializeObject(updateAccountDto);
            var jsonDocument = JsonDocument.Parse(jsonUpdatedProduct);
            JsonElement account = jsonDocument.RootElement;

            //Act
            var result = (OkObjectResult)await _accountController.UpdateAccount("672d61c84e4eeed22aad9f8b", account);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async void AccountController_SoftDelete_ReturnSuccess()
        {
            //Arrange
            var updateAccountDto = CreateFakeUpdateAccountDTO();
            updateAccountDto.Status = AccountStatusEnum.INACTIVE.ToString();

            var jsonUpdatedProduct = JsonConvert.SerializeObject(updateAccountDto);
            var jsonDocument = JsonDocument.Parse(jsonUpdatedProduct);
            JsonElement account = jsonDocument.RootElement;

            //Act
            var result = (OkObjectResult)await _accountController.SoftDeleteAccount("672d61c84e4eeed22aad9f8b", account);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void AccountController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _accountController.DeleteAccount("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
