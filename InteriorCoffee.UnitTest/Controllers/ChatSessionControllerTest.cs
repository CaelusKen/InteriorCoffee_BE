using FakeItEasy;
using FluentAssertions;
using InteriorCoffee.Application.DTOs.ChatMessage;
using InteriorCoffee.Application.DTOs.ChatSession;
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
    public class ChatSessionControllerTest
    {
        private readonly ILogger<ChatSessionController> logger;
        private readonly IChatSessionService _chatSessionService;
        private readonly ChatSessionController _chatSessionController;

        public ChatSessionControllerTest()
        {
            logger = A.Fake<ILogger<ChatSessionController>>();
            _chatSessionService = A.Fake<IChatSessionService>();
            _chatSessionController = new ChatSessionController(logger, _chatSessionService);
        }

        private static CreateChatSessionDTO CreateFakeCreateChatSessionDTO() => A.Fake<CreateChatSessionDTO>();
        private static UpdateChatSessionDTO CreateFakeUpdateChatSessionDTO() => A.Fake<UpdateChatSessionDTO>();
        private static AddChatMessageDTO CreateFakeAddedMessageDTO() => A.Fake<AddChatMessageDTO>();
        private static UpdateChatMessageDTO CreateFakeUpdateMessageDTO() => A.Fake<UpdateChatMessageDTO>();


        #region Chat Session Function
        #region Get Function Test
        [Fact]
        public async void ChatSessionController_GetAllChatSessions_ReturnChatSessionList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _chatSessionController.GetAllChatSessions();

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<List<ChatSession>>();
        }

        [Fact]
        public async void ChatSessionController_GetChatSessionById_ReturnChatSession()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _chatSessionController.GetChatSessionById("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<ChatSession>();
        }

        [Fact]
        public async void ChatSessionController_GetMerchantChatSessions_ReturnChatSessionList()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _chatSessionController.GetMerchantChatSessions("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeAssignableTo<List<ChatSession>>();
        }
        #endregion

        #region Create Function Test
        [Fact]
        public async void ChatSessionController_Create_ReturnSuccess()
        {
            //Arrange
            var createChatSessionDto = CreateFakeCreateChatSessionDTO();

            //Act
            var result = (OkObjectResult)await _chatSessionController.CreateChatSession(createChatSessionDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Update Function Test
        [Fact]
        public async void ChatSessionController_Update_ReturnSuccess()
        {
            //Arrange
            var updateChatSessionDto = CreateFakeUpdateChatSessionDTO();

            //Act
            var result = (OkObjectResult)await _chatSessionController.UpdateChatSession("672d61c84e4eeed22aad9f8b", updateChatSessionDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion

        #region Delete Function Test
        [Fact]
        public async void ChatSessionController_Delete_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _chatSessionController.DeleteChatSession("672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
        #endregion

        #region Chat Message Function
        [Fact]
        public async void ChatSessionController_SendChatMessage_ReturnSuccess()
        {
            //Arrange
            var sendMessageDto = CreateFakeAddedMessageDTO();

            //Act
            var result = (OkObjectResult)await _chatSessionController.SendChatMessage("672d61c84e4eeed22aad9f8b", sendMessageDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async void ChatSessionController_UpdateChatMessage_ReturnSuccess()
        {
            //Arrange
            var sendMessageDto = CreateFakeUpdateMessageDTO();

            //Act
            var result = (OkObjectResult)await _chatSessionController.UpdateChatMessage("672d61c84e4eeed22aad9f8b", sendMessageDto);

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }

        [Fact]
        public async void ChatSessionController_DeleteChatMessage_ReturnSuccess()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _chatSessionController.DeleteChatMessage("672d61c84e4eeed22aad9f8b", "672d61c84e4eeed22aad9f8b");

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        #endregion
    }
}
