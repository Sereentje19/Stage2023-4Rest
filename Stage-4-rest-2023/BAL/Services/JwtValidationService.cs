using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Stage4rest2023.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the JwtValidationService class with the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration containing JWT settings.</param>
        public JwtValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) with the specified claims and expiration time.
        /// </summary>
        /// <remarks>
        /// This method creates a JWT token using the provided configuration settings for the issuer, audience,
        /// and signing key, and sets an expiration time of 30 minutes from the current time.
        /// </remarks>
        /// <returns>The generated JWT token as a string.</returns>
        public string GenerateToken(LoginRequestDTO user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email)
            };
            
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}