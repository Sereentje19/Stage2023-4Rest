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

        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <returns>An IActionResult containing the list of users as UserResponseDto.</returns>
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
        
        /// <summary>
        /// Creates a new user based on the provided CreateUserRequestDto.
        /// </summary>
        /// <param name="userRequest">The CreateUserRequestDto containing information for creating the user.</param>
        /// <returns>An IActionResult indicating the success of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto userRequest)
        {
            await _userService.CreateUserAsync(userRequest);   
            return Ok(new { message = "Gebruiker toegevoegd." });
        }
        
        /// <summary>
        /// Updates user information based on the provided UpdateUserRequestDto.
        /// </summary>
        /// <param name="updateUserRequestDto">The UpdateUserRequestDto containing information for updating the user.</param>
        /// <returns>An IActionResult indicating the success of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            await _userService.UpdateUserAsync(updateUserRequestDto);   
            return Ok(new { message = "Gebruiker geupdate." });
        }
        
        /// <summary>
        /// Deletes a user based on the provided email address.
        /// </summary>
        /// <param name="email">The email address of the user to be deleted.</param>
        /// <returns>An IActionResult indicating the success of the operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            await _userService.DeleteUserAsync(email);   
            return Ok(new { message = "Gebruiker verwijderd." });
        }
    }
}