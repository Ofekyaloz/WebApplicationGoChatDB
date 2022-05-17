﻿#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IWebService _context;

        public string CookieAuthentication { get; private set; }

        public UsersController(IWebService context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            User user = _context.getUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("/api/Users/Register")]
        public async Task<IActionResult> Register([Bind("Username,Password,Nickname,Email,Photo")] User user)
        {
            var q = from u in _context.getUsers()
                    where u.Username == user.Username
                    select u;

            if (q.Count() > 0)
            {
                return NotFound();
            }

            else
            {
                SignIn(user);
                _context.addUser(user);
                return Ok();
            }
        }
        public class loginFields
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        [Route("/api/Users/Login")]
        public async Task<IActionResult> Login([Bind("username,password")] loginFields loginFields)
        {
            var q = from u in _context.getUsers()
                    where u.Username == loginFields.username && u.Password == loginFields.password
                    select u;

            if (q.Count() > 0)
            {
                SignIn(q.First());
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        private async void Logout()
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
