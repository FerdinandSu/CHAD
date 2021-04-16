using System.Threading.Tasks;
using Chad.Models;
using Chad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChadApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ChadController
    {
        public ChatController(ChadManager chadManager)
        {
            ChadManager = chadManager;
        }

        private ChadManager ChadManager { get; }

        [HttpGet("")]
        public async Task<ActionResult<ChatMessage[]>> Get()
        {
            return await ChadManager.GetChatsAsync();
        }

        [HttpPost("")]
        public async Task<ActionResult> Post([FromBody] ChatMessage message)
        {
            return await ChadManager.PostChatAsync(message) ? NoContent() : Forbid();
        }
    }
}