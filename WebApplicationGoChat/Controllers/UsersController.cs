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
        private readonly WebApplicationGoChatContext _context;

        public string CookieAuthentication { get; private set; }

        public UsersController(WebApplicationGoChatContext context)
        {
            _context = context;
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
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Username == user.Username
                        select u;

                if (q.Count() > 0)
                {
                    ViewData["Error"] = "This user is already exists!";
                }

                else
                {
                    SignIn(user);
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Contacts");
                }
            }
            return RedirectToAction("Index", "Contacts");
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
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Username == user.Username && u.Password == user.Password
                        select u;

                if (q.Count() > 0)
                {
                    SignIn(q.First());
                    return RedirectToAction(nameof(Index), "Contacts");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are wrong!";
                }
            }
            return RedirectToAction("Login", "Users");
        }

        public async void Logout()
        {
            HttpContext.SignOutAsync();
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
