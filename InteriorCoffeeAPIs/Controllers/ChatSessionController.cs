using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.ChatSession;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class ChatSessionController : BaseController<ChatSessionController>
    {
        private readonly IChatSessionService _chatSessionService;

        public ChatSessionController(ILogger<ChatSessionController> logger, IChatSessionService chatSessionService) : base(logger)
        {
            _chatSessionService = chatSessionService;
        }

        [HttpGet(ApiEndPointConstant.ChatSession.ChatSessionsEndpoint)]
        [ProducesResponseType(typeof(List<ChatSession>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all chat sessions")]
        public async Task<IActionResult> GetAllChatSessions()
        {
            var result = await _chatSessionService.GetChatSessionListAsync();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.ChatSession.ChatSessionEndpoint)]
        [ProducesResponseType(typeof(ChatSession), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a chat session by id")]
        public async Task<IActionResult> GetChatSessionById(string id)
        {
            var result = await _chatSessionService.GetChatSessionByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.ChatSession.ChatSessionsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create chat session")]
        public async Task<IActionResult> CreateChatSession(CreateChatSessionDTO chatSession)
        {
            await _chatSessionService.CreateChatSessionAsync(chatSession);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.ChatSession.ChatSessionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a chat session's data")]
        public async Task<IActionResult> UpdateChatSession(string id, [FromBody] UpdateChatSessionDTO updateChatSession)
        {
            var existingChatSession = await _chatSessionService.GetChatSessionByIdAsync(id);

            await _chatSessionService.UpdateChatSessionAsync(id, updateChatSession);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.ChatSession.ChatSessionEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a chat session")]
        public async Task<IActionResult> DeleteChatSession(string id)
        {
            var chatSession = await _chatSessionService.GetChatSessionByIdAsync(id);

            await _chatSessionService.DeleteChatSessionAsync(id);
            return Ok("Action success");
        }
    }
}
