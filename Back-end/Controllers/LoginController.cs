using Microsoft.AspNetCore.Mvc;
using Back_end.Models;
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
        public ActionResult Login([FromBody] User user)
        {
            try
            {
                User currentUser = loginService.checkCredentials(user);
                var token = jwtValidationService.GenerateToken();
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}