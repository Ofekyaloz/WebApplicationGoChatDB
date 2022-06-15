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
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            var tmp = await _context.getContacts(userId);
            return Ok(tmp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;

            Contact contact = await _context.getContact(userId, id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("id,name,server")] AddContactFields contactFields)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            _context.addContact(userId, contactFields);
            return Ok();
        }


        [HttpPut("{id}")]
        
        public async Task<IActionResult> Edit(string id, [Bind("name,server")] UpdateContactFields contactFields)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            _context.editContact(userId, id, contactFields);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            _context.removeContact(userId, id);

            return Ok();
        }        
    }
}
