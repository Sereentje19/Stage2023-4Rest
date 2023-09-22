using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Back_end.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
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
        public ActionResult Login([FromBody] User user)
        {
            try
            {
                User currentUser = userService.checkCredentials(user);

                if (currentUser != null)
                {
                    var token = GenerateToken();
                    return Ok(token);
                }
                return NotFound("Email of wachtwoord incorrect");
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}