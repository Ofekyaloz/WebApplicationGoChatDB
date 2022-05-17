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
        public async Task<IActionResult> Create([Bind("id,name,server")] AddContactFields contactFields)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.addContact(userId, contactFields);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [Bind("name,server")] UpdateContactFields contactFields)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.editContact(userId, id, contactFields);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            _context.removeContact(userId, id);

            return Ok();
        }

        public class Invitation
        {
            public string from { get; set; }
            public string to { get; set; }
            public string server { get; set; }
        }

        [HttpPost]
        [Route("api/invitations")]
        public async Task<IActionResult> Create([Bind("from,to,server")] Invitation invitation)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            List<Contact> contacts = _context.getUsers().Find(m => m.Username == userId).Contacts;
            Contact contact = contacts.Find(m => m.id == invitation.to);

            if (contact != null)
                return NotFound();

            if (_context.getUsers().Find(m => m.Username == invitation.from) == null)
                return NotFound();

            contacts.Add(new Contact() { server = invitation.server, Messages = new List<Message>(), lastdate = null, last = null, name = invitation.from });

            return Ok();
        }

        [HttpPost]
        [Route("api/transfer")]

        public async Task<IActionResult> Create([Bind("from,to,content")] string from, string to, string content)
        {
            return Ok();
        }

    }
}
