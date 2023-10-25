using ChatSignalR.API.Model;
using ChatSignalR.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatSignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatservice;
        public ChatController(ChatService chatService)
        {
            _chatservice= chatService;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDto model)
        {
            if (_chatservice.AddUserToList(model.Name))
            {
                return NoContent();
            }
            return BadRequest("404");
        }




    }
}
