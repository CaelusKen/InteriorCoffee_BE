using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffeeAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ChatSessionController : ControllerBase
    {
        private readonly IChatSessionService _chatSessionService;

        public ChatSessionController(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }

        // GET: api/ChatSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatSession>>> GetChatSessions()
        {
            var chatSessions = await _chatSessionService.GetAllChatSessionsAsync();
            return Ok(chatSessions);
        }

        // GET: api/ChatSession/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatSession>> GetChatSession(string id)
        {
            var chatSession = await _chatSessionService.GetChatSessionByIdAsync(id);
            if (chatSession == null)
            {
                return NotFound();
            }
            return Ok(chatSession);
        }

        // POST: api/ChatSession
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<ChatSession>> CreateChatSession([FromBody] ChatSession chatSession)
        {
            // Ensure the _id is not set
            chatSession._id = null;

            await _chatSessionService.CreateChatSessionAsync(chatSession);
            return CreatedAtAction(nameof(GetChatSession), new { id = chatSession._id }, chatSession);
        }

        // PUT: api/ChatSession/{id}
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateChatSession(string id, [FromBody] ChatSession chatSession)
        {
            if (id != chatSession._id)
            {
                return BadRequest();
            }

            var existingChatSession = await _chatSessionService.GetChatSessionByIdAsync(id);
            if (existingChatSession == null)
            {
                return NotFound();
            }

            await _chatSessionService.UpdateChatSessionAsync(id, chatSession);
            return NoContent();
        }

        // DELETE: api/ChatSession/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatSession(string id)
        {
            var chatSession = await _chatSessionService.GetChatSessionByIdAsync(id);
            if (chatSession == null)
            {
                return NotFound();
            }

            await _chatSessionService.DeleteChatSessionAsync(id);
            return NoContent();
        }
    }
}
