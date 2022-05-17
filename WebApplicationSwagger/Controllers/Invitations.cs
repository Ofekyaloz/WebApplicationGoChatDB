using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly WebService _context;

        public InvitationsController(WebService context)
        {
            _context = new WebService();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("from,to,server")] string from, string to, string server)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            List<Contact> contacts = _context.getUsers().Find(m => m.Username == userId).Contacts;
            Contact contact = contacts.Find(m => m.id == from);

            if (contact == null)
                return NotFound();
            else
            {

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
