using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Back_end.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Back_end.Services;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IUserService userService;
        public LoginController(IConfiguration config, IUserService us)
        {
            _config = config;
            userService = us;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] User userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private User Authenticate(User userLogin)
        {
            User currentUser = userService.checkCredentials(userLogin);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}