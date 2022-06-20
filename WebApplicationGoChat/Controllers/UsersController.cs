#nullable disable
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplicationGoChat.Models;
using WebApplicationGoChat.Services;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IWebService _context;
        private IConfiguration _configuration;

        public string CookieAuthentication { get; private set; }

        public UsersController(IWebService context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            User user = await _context.getUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("/api/Users/Register")]
        public async Task<IActionResult> Register([Bind("Username,Password,Nickname,Email,Photo,Contacts, Connection")] User user)
        {
            var q = from u in await _context.getUsers()
                    where u.Username == user.Username
                    select u;

            if (q.Count() > 0)
            {
                return NotFound();
            }
            
            var token = SignIn(user);
            await _context.addUser(user);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            
        }
        public class LoginFields
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        [Route("/api/Users/Login")]
        public async Task<IActionResult> Login([Bind("username,password")] LoginFields loginFields)
        {
            var q = from u in await _context.getUsers()
                    where u.Username == loginFields.username && u.Password == loginFields.password
                    select u;

            if (q.Count() > 0)
            {
                
                var token = SignIn(q.First());
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return NotFound();
            
        }

        public class ConnectionId
        {
            public string connectionId { get; set; }
        }

        [HttpPost]
        [Authorize]
        [Route("/api/Users/Connection")]
        public async Task<IActionResult> Connection([Bind("connectionId")] ConnectionId connection)
        {
            var userId = HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            if (userId == null)
            {
                return NotFound();
            }
            (await _context.getUser(userId)).Connection = connection.connectionId;
            return Ok();
        }


        private async void Logout()
        {
            await HttpContext.SignOutAsync();
        }

        private JwtSecurityToken SignIn(User user)
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWTParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId",user.Username)
                };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWTParams:Issuer"],
                _configuration["JWTParams:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: mac);
            return token;
        }
    }
}
