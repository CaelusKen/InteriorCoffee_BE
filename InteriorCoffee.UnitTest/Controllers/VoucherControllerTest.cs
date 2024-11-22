using Amazon.Runtime.Internal.Util;
using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
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
    public class VoucherControllerTest
    {
        private readonly IVoucherService _voucherService;
        private readonly VoucherController _voucherController;
        private readonly ILogger<VoucherController> logger;

        public VoucherControllerTest()
        {
            logger = A.Fake<ILogger<VoucherController>>(); 
            this._voucherService = A.Fake<IVoucherService>();
            this._voucherController = new VoucherController(logger, this._voucherService);
        }

        private static Voucher CreateFakeVoucher() => A.Fake<Voucher>();
        private static CreateVoucherDTO CreateFakeCreateVoucherDto() => A.Fake<CreateVoucherDTO>();
        private static UpdateVoucherDTO CreateFakeUpdateVoucherDto() => A.Fake<UpdateVoucherDTO>();

        #region Get Function Test
        [Fact]
        public async void VoucherController_GetVouchers_ReturnVoucherList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _voucherController.GetVouchers(1, 10);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Paginate<Voucher>>();
        }

        [Fact]
        public async void VoucherController_GetVoucherById_ReturnVoucher()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _voucherController.GetVoucherById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void VoucherController_Create_ReturnSuccess()
        {
            //Arrange
            var createVoucherDto = CreateFakeCreateVoucherDto();

            //Act
            var result = (OkObjectResult)await _voucherController.CreateVoucher(createVoucherDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void VoucherController_Update_ReturnSuccess()
        {
            //Arrange
            var updateVoucherDto = CreateFakeUpdateVoucherDto();

            //Act
            var result = (OkObjectResult)await _voucherController.UpdateVouchers("672d61c84e4eeed22aad9f8b", updateVoucherDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void VoucherController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _voucherController.DeleteVouchers("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
