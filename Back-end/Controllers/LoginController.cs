using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Back_end.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using System.Security.Principal;

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

        /// <summary>
        /// wat doet de functie
        ///  wat die returnt
        /// </summary>
        /// <returns></returns>
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


        public static string SecretKey { get; } = "iRAW38828BzlnM3tJFcPiuCmZdUcM9ng";

        public static string CheckForJwt(HttpContext context, IConfiguration configuration)
        {
            string authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                throw new Exception("No token provided");
            }

            // Extract JWT token from the header
            string jwtToken = authHeader.Substring("Bearer ".Length);

            // Retrieve JWT configuration from app settings
            var jwtSettings = configuration.GetSection("Jwt");

            // Create token validation parameters using configuration values
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // Validate and decode JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var securityToken);
                return claimsPrincipal.Identity.Name; // Assuming you want to retrieve the subject (name) from the token
            }
            catch (SecurityTokenExpiredException)
            {
                throw new Exception("Token expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new Exception("Invalid token signature");
            }
            catch (Exception)
            {
                throw new Exception("Invalid token");
            }
        }

    }
}