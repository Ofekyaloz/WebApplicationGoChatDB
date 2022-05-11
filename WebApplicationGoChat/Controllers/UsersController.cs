#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApplicationGoChat.Data;
using WebApplicationGoChat.Models;

namespace WebApplicationGoChat.Controllers
{
    public class UsersController : Controller
    {
        public static List<User> _users = new List<User>();
        // private readonly WebApplicationGoChatContext _context;

        public string CookieAuthentication { get; private set; }

        public UsersController(WebApplicationGoChatContext context)
        {
            _users.Add(new User()
            {
                Username = "giligutfeld",
                Password = "123456",
                Email = "gili@gmail.com",
                Nickname = "Gili",
                Photo = "./Pictures/cat.jpg",
                Contacts = new List<Contact>() { new Contact() { id = "ofekyaloz", name = "Ofek", server = "localhost:7265", last = "Hello!", lastdate = "1-1-2022" },
                new Contact() { id = "leomessi", name = "Leo", server = "localhost:7265", last = "Hi!", lastdate = "15-1-2022" }},
                Messages = new Dictionary<string, List<Message>>() { { "ofekyaloz", new List<Message>() { new Message { id = 1, content = "Hi", created = "3-1-2022", sent = true } } } }
            });

            _users.Add(new User()
            {
                Username = "ofekyaloz",
                Password = "234567",
                Email = "ofek@gmail.com",
                Nickname = "Ofek",
                Photo = "./Pictures/cat.jpg",
                Contacts = new List<Contact>() { new Contact() { id = "giligutfeld", name = "Gili", server = "localhost:7265", last = "Hello!", lastdate = "1-1-2022" } },
                Messages = new Dictionary<string, List<Message>>() { { "giligutfeld", new List<Message>() { new Message { id = 1, content = "Hello", created = "1-1-2022", sent = false } } } }
            });
            _users.Add(new User() { Username = "noakirel", Password = "111111", Email = "noa@gmail.com", Nickname = "Noa", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>(), Messages = new Dictionary<string, List<Message>>() });
            _users.Add(new User() { Username = "omeradam", Password = "shigramefoeret", Email = "omer@gmail.com", Nickname = "Omer", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>(), Messages = new Dictionary<string, List<Message>>() });
            _users.Add(new User() { Username = "bibinetanyahu", Password = "bbbbbb", Email = "bibi@gmail.com", Nickname = "Bibi", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>(), Messages = new Dictionary<string, List<Message>>() });
            _users.Add(new User() { Username = "edenhason", Password = "shemishuyaazoroti", Email = "eden@gmail.com", Nickname = "Eden", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>(), Messages = new Dictionary<string, List<Message>>() });
            _users.Add(new User() { Username = "leomessi", Password = "101010", Email = "leo@gmail.com", Nickname = "Messi", Photo = "./Pictures/cat.jpg", Contacts = new List<Contact>(), Messages = new Dictionary<string, List<Message>>() });
            // _context = context;
        }

        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,Nickname,Email,Photo")] User user)
        {
            var q = from u in _users
                    where u.Username == user.Username
                    select u;

            if (q.Count() > 0)
            {
                ViewData["Error"] = "This user is already exists!";
            }

            else
            {
                SignIn(user);
                _users.Add(user);
                return RedirectToAction("api/{Contacts}");
            }
            return RedirectToAction("Register", "Users");
        }

        // GET: Users/Create
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] User user)
        {
            var q = from u in _users
                    where u.Username == user.Username && u.Password == user.Password
                    select u;

            if (q.Count() > 0)
            {
                SignIn(q.First());
                // return RedirectToAction("api/Contacts");
            }
            else
            {
                ViewData["Error"] = "Username and/or password are wrong!";
            }
            return RedirectToAction("Login", "Users");
        }

        public async void Logout()
        {
            await HttpContext.SignOutAsync();
        }

        private async void SignIn(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //ExpiredUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
