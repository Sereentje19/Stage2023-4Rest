using BLL.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using BLL.Services;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtValidationService _jwtValidationService;

        public UserController(IUserService userService, IJwtValidationService jwtValidationService)
        {
            _userService = userService;
            _jwtValidationService = jwtValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            IEnumerable<UserResponseDto> users = await _userService.GetAllUsersAsync();   
            return Ok(users);
        }
        
        /// <summary>
        /// Handles user login and returns an authentication token.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>An authentication token if login is successful; otherwise, an error message.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto userDto)
        {
            await _userService.CheckCredentialsAsync(userDto);
            User user =  await _userService.GetUserByEmailAsync(userDto.Email);
            string token = _jwtValidationService.GenerateToken(user);
            return Ok(token);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto userRequest)
        {
            await _userService.CreateUserAsync(userRequest);   
            return Ok(new { message = "Gebruiker toegevoegd." });
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            await _userService.UpdateUserAsync(updateUserRequestDto);   
            return Ok(new { message = "Gebruiker geupdate." });
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            await _userService.DeleteUserAsync(email);   
            return Ok(new { message = "Gebruiker verwijderd." });
        }
    }
}