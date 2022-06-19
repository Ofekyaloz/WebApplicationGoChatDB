using Microsoft.AspNetCore.Mvc;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {

        private readonly IWebService _context;

        public InvitationsController(IWebService context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("/api/invitations")]
        public async Task<IActionResult> Create([FromBody] Invitation invitation)
        {
            await _context.addContact(invitation.to, new AddContactFields()
            {
                id = invitation.from,
                server = invitation.server,
                name = (await _context.getUsers()).Find(m => m.Username == invitation.from)?.Nickname
            });
            return Ok();
        }
    }
}
