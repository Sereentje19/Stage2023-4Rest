using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using PL.Models.Requests;
using BLL.Services;
using PL.Models;

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

        public LoginController(ILoginService loginService, IJwtValidationService jwtValidationService)
        {
            _loginService = loginService;
            _jwtValidationService = jwtValidationService;
        }

        /// <summary>
        /// Handles user login and returns an authentication token.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>An authentication token if login is successful; otherwise, an error message.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO userDto)
        {
            await _loginService.CheckCredentials(userDto);
            User user =  await _loginService.GetUserByEmail(userDto.Email);
            string token = _jwtValidationService.GenerateToken(user);
            return Ok(token);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string email, User user, bool updateName)
        {
            await _loginService.PutUser(user, email, updateName);   
            return Ok(new { message = "User updated" });
        }
        
    }
}