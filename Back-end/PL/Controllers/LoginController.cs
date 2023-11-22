using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using PL.Models.Requests;
using BLL.Services;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("login")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IJwtValidationService _jwtValidationService;
        private readonly IMailService _mailService;

        public LoginController(ILoginService loginService, IJwtValidationService jwtValidationService,
            IMailService mailService)
        {
            _loginService = loginService;
            _jwtValidationService = jwtValidationService;
            _mailService = mailService;
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