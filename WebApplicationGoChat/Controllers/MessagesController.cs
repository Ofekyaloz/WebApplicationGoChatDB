#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contacts/{id}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IWebService _context;

        public MessagesController(IWebService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            List<Message> messages = _context.getMessasges(userId, id);
            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        [HttpGet("{id2}")]
        public async Task<IActionResult> Details(string id, int id2)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            Message message = _context.getMessasge(userId, id, id2);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, [Bind("content")] string content)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            _context.addMessage(userId, id, content);
            return Ok();
        }

        [HttpPut("{id2}")]
        public async Task<IActionResult> Edit(string id, int id2, [Bind("content")] string content)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;

            _context.editMessage(userId, id, id2, content);
            return Ok();
        }

        [HttpDelete("{id2}")]
        public async Task<IActionResult> DeleteConfirmed(string id, int id2)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            _context.removeMessage(userId, id, id2);

            return Ok();
        }
    }
}
