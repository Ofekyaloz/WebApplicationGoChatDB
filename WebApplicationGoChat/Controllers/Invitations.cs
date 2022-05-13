using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Invitations : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("from,to,server")] string from, string to, string server)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            List<Contact> contacts = UsersController._users.Find(m => m.Username == userId).Contacts;
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
