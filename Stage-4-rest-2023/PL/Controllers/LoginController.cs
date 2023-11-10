using Microsoft.AspNetCore.Mvc;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("login")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IJwtValidationService _jwtValidationService;

        public LoginController(ILoginService loginService, IJwtValidationService jwtValidationService)
        {
            _loginService = loginService;
            _jwtValidationService = jwtValidationService;
        }

        /// <summary>
        /// Handles user login and returns an authentication token.
        /// </summary>
        /// <param name="user">The user credentials for login.</param>
        /// <returns>An authentication token if login is successful; otherwise, an error message.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO user)
        {
            await _loginService.CheckCredentials(user);
            String token = _jwtValidationService.GenerateToken(user);
            return Ok(token);
        }
    }
}