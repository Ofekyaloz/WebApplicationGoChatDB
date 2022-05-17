#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationGoChat.Data;
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
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
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
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            Message message = _context.getMessasge(userId, id, id2);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("content")] Message message)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            message.created = DateTime.Now.ToString();
            message.sent = true;

            _context.addMessage(userId, id, message);
            return Ok();
        }

        [HttpPut("{id2}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, int id2, [Bind("content")] Message message)
        {
            if (id2 != message.id)
            {
                return NotFound();
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            message.id = id2;

            _context.editMessage(userId, id, message);

            return Ok();
        }

        [HttpDelete("{id2}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int id2)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.removeMessage(userId, id, id2);

            return Ok();
        }
    }
}
