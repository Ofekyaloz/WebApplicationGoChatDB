using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplicationGoChat.Hubs;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;
using static WebApplicationGoChat.Controllers.ContactsController;
using Message = FirebaseAdmin.Messaging.Message;

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

            await _context.addMessage(transfer.to, transfer.from, transfer.content, false);
            
            var contact = await _context.getContact(user.Username, transfer.from);

            if (contact == null)
                return NotFound();

            if (user.Connection != null)
            {
                await _hub.Clients.Client(user.Connection).SendAsync("MessageReceived", "ho");
            }

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential =
                        GoogleCredential.FromFile(
                            "..\\WebApplicationGoChat\\go-chat-android-firebase-adminsdk-ta6uo-a3cefa54e7.json")
                });
            }

            if (user?.Token != null)
            {
                var message = new FirebaseAdmin.Messaging.Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"myData", "1337"}, {"From", transfer.from}
                    },
                    Notification = new Notification()
                    {
                        Body = transfer.content,
                        Title = "Message from: " + transfer.from
                    },
                    Token = user.Token
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Successfully " + response);
            }
            
            return Ok();
        }
    }
}
