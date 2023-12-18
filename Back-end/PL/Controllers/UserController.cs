using BLL.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using BLL.Services;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IJwtValidationService _jwtValidationService;

        public UserController(ILoginService loginService, IJwtValidationService jwtValidationService)
        {
            _loginService = loginService;
            _jwtValidationService = jwtValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserResponse> users = await _loginService.GetAllUsers();   
            return Ok(users);
        }
        
        /// <summary>
        /// Handles user login and returns an authentication token.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>An authentication token if login is successful; otherwise, an error message.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO userDto)
        {
            await _loginService.CheckCredentials(userDto);
            User user =  await _loginService.GetUserByEmail(userDto.Email);
            string token = _jwtValidationService.GenerateToken(user);
            return Ok(token);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            await _loginService.PostUser(user);   
            return Ok(new { message = "Gebruiker toegevoegd." });
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string email, User user, bool updateName)
        {
            await _loginService.PutUser(user, email, updateName);   
            return Ok(new { message = "Gebruiker geupdate." });
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _loginService.DeleteUser(email);   
            return Ok(new { message = "Gebruiker verwijderd." });
        }
    }
}