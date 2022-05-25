using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplicationGoChat.Hubs;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;
using static WebApplicationGoChat.Controllers.ContactsController;

namespace WebApplicationGoChat.Controllers
{
    public class TransferController : ControllerBase
    {
        private readonly IWebService _context;
        private readonly IHubContext<MyHub> _hub;

        public TransferController(IWebService service, IHubContext<MyHub> messageHub)
        {
            _context = service;
            _hub = messageHub;
        }


        [HttpPost]
        [Route("/api/transfer")]
        public async Task<IActionResult> Create([FromBody] Transfer transfer)
        {
            _context.addMessage(transfer.to, transfer.from, transfer.content, false);
            User user = _context.getUser(transfer.to);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Connection != null)
            {
                _hub.Clients.Client(user.Connection).SendAsync("MessageReceived", "ho");
            }
            return Ok();
        }
    }
}
