using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Back_end.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private readonly IConfiguration _configuration;
        public static string SecretKey { get; } = "iRAW38828BzlnM3tJFcPiuCmZdUcM9ng";

        public JwtValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// wat doet de functie
        ///  wat die returnt
        /// </summary>
        /// <returns></returns>
        public string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string ValidateToken(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                throw new Exception("No token provided");
            }

            // Extract JWT token from the header
            string jwtToken = authHeader.Substring("Bearer ".Length);

            // Create token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // Validate and decode JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);

                // Assuming you want to retrieve the subject (name) from the token
                return claimsPrincipal.Identity.Name;
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