using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
            User user = await _context.getUser(transfer.to);

            if (user == null)
            {
                return NotFound();
            }

            var app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("WebApplicationGoChat\\go-chat-android-firebase-adminsdk-ta6uo-a3cefa54e7.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging%22")
            });

            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
            if (user?.FirebaseToken != null)
            {
                await messaging.SendAsync(new FirebaseAdmin.Messaging.Message
                {
                    Notification = new Notification()
                    {
                        Body = transfer.content,
                        Title = transfer.from
                    },
                    Token = user.FirebaseToken
                });
            }

            var contact = await _context.getContact(user.Username, transfer.from);

            if (contact == null)
                return NotFound();

            await _context.addMessage(transfer.to, transfer.from, transfer.content, false);

            Uri uri = new Uri($"https://localhost:7225/api/Contacts/%7Bcontact.Id%7D/messages/%7Bmessage.Id%7D%22");

            if (user.Connection != null)
            {
                await _hub.Clients.Client(user.Connection).SendAsync("MessageReceived", "ho");
            }
            return Ok();
        }
    }
}
