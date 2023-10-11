using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Back_end.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Back_end.Services
{
    public class JwtValidationService : IJwtValidationService
    {
        private readonly IConfiguration _configuration;
        public static string SecretKey { get; } = "iRAW38828BzlnM3tJFcPiuCmZdUcM9ng";

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
        public string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validates and decodes a JSON Web Token (JWT) extracted from the HTTP context's Authorization header.
        /// </summary>
        /// <param name="context">The HTTP context containing the Authorization header with the JWT token.</param>
        /// <returns>
        /// The name (subject) extracted from the JWT token if validation succeeds.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the JWT token is missing or in an invalid format, or if the token validation fails.
        /// The exception message provides guidance for reauthentication.
        /// </exception>
        public string ValidateToken(HttpContext context)
        {
            try
            {
                string jwtToken = ExtractJwtToken(context);
                var tokenValidationParameters = ConfigureTokenValidationParameters();

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);

                return claimsPrincipal.Identity.Name;
            }
            catch (Exception)
            {
                throw new TokenValidationException("Opnieuw inloggen vereist");
            }
        }

        /// <summary>
        /// Extracts a JSON Web Token (JWT) from the Authorization header of the provided HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context containing the Authorization header.</param>
        /// <returns>
        /// The JWT token extracted from the Authorization header.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the Authorization header is missing or not in the expected "Bearer [Token]" format.
        /// The exception message indicates that login is required.
        /// </exception>
        private string ExtractJwtToken(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                throw new InvalidJwtTokenException("inloggen vereist");
            }

            return authHeader.Substring("Bearer ".Length);
        }

        /// <summary>
        /// Configures the parameters used for validating JSON Web Tokens (JWTs).
        /// </summary>
        /// <returns>
        /// An instance of <see cref="TokenValidationParameters"/> configured with specific settings for JWT validation.
        /// </returns>
        private TokenValidationParameters ConfigureTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}