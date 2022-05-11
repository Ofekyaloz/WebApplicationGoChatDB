#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationGoChat.Data;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contacts/{id}/[controller]")]
    public class MessagesController : Controller
    {
        private readonly WebApplicationGoChatContext _context;

        public MessagesController(WebApplicationGoChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            Dictionary<string, List<Message>> dict = UsersController._users.Find(m => m.Username == userId).Messages;
            List<Message> messages = dict[id];

            return Json(messages);
        }

        [HttpGet("{id2}")]
        public async Task<IActionResult> Details(string id, int? id2)
        {
            if (id2 == null)
            {
                return NotFound();
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            Dictionary<string, List<Message>> dict = UsersController._users.Find(m => m.Username == userId).Messages;
            List<Message> messages = dict[id];
            Message message = messages.Find(m => m.id == id2);

            if (message == null)
            {
                return NotFound();
            }

            return Json(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,content,created,sent")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        [HttpPut("{id2}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id2, [Bind("id,content,created,sent")] Message message)
        {
            if (id2 != message.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        [HttpDelete("{id2}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id2)
        {
            var message = await _context.Message.FindAsync(id2);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.id == id);
        }
    }
}
