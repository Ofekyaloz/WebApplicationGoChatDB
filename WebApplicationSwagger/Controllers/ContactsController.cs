#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IWebService _context;
        public ContactsController(IWebService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(_context.getContacts(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            Contact contact = _context.getContact(userId, id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,server")] Contact contact)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.addContact(userId, contact);
            return Ok();
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("name,server")] Contact contact)
        {
            if (id != contact.id)
            {
                return NotFound();
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.editContact(userId, contact);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.removeContact(userId, id);

            return RedirectToAction(nameof(Index));
        }
    }
}
