using Microsoft.AspNetCore.Mvc;
using KisanGuru.Models.Entities;
using KisanGuru.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using KisanGuru.Data;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace KisanGuru.Controllers
{
    [Route("v1/api/kisan_mitra/UserLogin")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<LoginController> _logger;
        private IConfiguration _config;

        public LoginController(IConfiguration configuration, ApplicationDbContext dbContext, ILogger<LoginController> logger)
        {
            _config = configuration;
            this.dbContext = dbContext;
            _logger = logger;
        }

        private UserLogin AuthenticateUser(UserLoginDto userlogindto)
        {
            UserLogin authenticatedUser = null;

            // Proper user authentication
            var user = dbContext.UserLogins.SingleOrDefault(u => u.user_name == userlogindto.user_name && u.password == userlogindto.password);
            if (user != null)
            {
                authenticatedUser = user;
            }

            return authenticatedUser;
        }

        private string GenerateToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.user_name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLoginDto userLogin)
        {
            IActionResult response = Unauthorized();
            var authenticatedUser = AuthenticateUser(userLogin);
            if (authenticatedUser != null)
            {
                var token = GenerateToken(authenticatedUser);
                response = Ok(new { token = token });
            }
            return response;
        }
    }
}
