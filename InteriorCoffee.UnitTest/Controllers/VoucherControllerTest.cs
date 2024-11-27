using Amazon.Runtime.Internal.Util;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.Voucher;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Paginate;
using InteriorCoffeeAPIs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Frameworks;
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

        private List<Voucher> mockVouchers = new List<Voucher>
        {
            new Voucher()
            {
                _id = "1",
                Code = "",
                Name  = "",
                Description = "" ,
                DiscountPercentage = 1,
                Status = "",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                MaxUse = 100,
                MinOrderValue = 1,
                UsedAccountIds = new List<string>(),
                Type = ""
             },
            new Voucher()
            {
                _id = "2",
                Code = "",
                Name  = "",
                Description = "" ,
                DiscountPercentage = 1,
                Status = "",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                MaxUse = 100,
                MinOrderValue = 1,
                UsedAccountIds = new List<string>(),
                Type = ""
             }
        };

        private static Voucher CreateFakeVoucher() => A.Fake<Voucher>();
        private static CreateVoucherDTO CreateFakeCreateVoucherDto() => A.Fake<CreateVoucherDTO>();
        private static UpdateVoucherDTO CreateFakeUpdateVoucherDto() => A.Fake<UpdateVoucherDTO>();

        #region Get Function Test
        //=========Success Case==========
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
            A.CallTo(() => _voucherService.GetVoucherById("1")).Returns(mockVouchers[0]);

            //Act
            var result = (OkObjectResult)await _voucherController.GetVoucherById("1");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<Voucher>();

            Assert.Equal(mockVouchers[0], result.Value);
        }

        //==========Fail Case============
        [Fact]
        public async void VoucherController_GetVoucherById_ReturnError()
        {
            //Arrange
            A.CallTo(() => _voucherService.GetVoucherById("999"))
                .Throws(() => new NotFoundException("Voucher id 999 cannot be found"));

            //Act
            //Get not exist voucher
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _voucherController.GetVoucherById("999"));

            //Assert
            Assert.Equal("Voucher id 999 cannot be found", exception.Message);
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
        //=========Success Cases
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

        //===========Fail Cases=========
        [Fact]
        public async void VoucherController_Update_ExceptionWhenUpdateNotFoundVoucher()
        {
            //Arrange
            UpdateVoucherDTO updateVoucherDTO = CreateFakeUpdateVoucherDto();

            A.CallTo(() => _voucherService.UpdateVoucher("999", updateVoucherDTO))
                .Throws(() => new NotFoundException("Voucher id 999 cannot be found"));

            //Act
            //Get not exist voucher
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _voucherController.UpdateVouchers("999", updateVoucherDTO));

            //Assert
            Assert.Equal("Voucher id 999 cannot be found", exception.Message);
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
