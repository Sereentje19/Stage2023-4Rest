using Microsoft.AspNetCore.Mvc;
using Stage4rest2023.Models;
using Microsoft.AspNetCore.Authorization;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IJwtValidationService jwtValidationService;

        /// <summary>
        /// Initializes a new instance of the LoginController class.
        /// </summary>
        /// <param name="ls">The login service for user authentication.</param>
        /// <param name="jwt">The JWT validation service for token generation.</param>
        public LoginController(ILoginService ls, IJwtValidationService jwt)
        {
            loginService = ls;
            jwtValidationService = jwt;
        }

        /// <summary>
        /// Handles user login and returns an authentication token.
        /// </summary>
        /// <param name="user">The user credentials for login.</param>
        /// <returns>An authentication token if login is successful; otherwise, an error message.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequestDTO user)
        {
            loginService.CheckCredentials(user);
            String token = jwtValidationService.GenerateToken();
            return Ok(token);
        }
    }
}