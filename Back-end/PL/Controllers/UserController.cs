﻿using BLL.Interfaces;
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
        public async Task<IActionResult> GetAllUsersAsync()
        {
            IEnumerable<UserResponseDto> users = await _loginService.GetAllUsersAsync();   
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
            await _loginService.CheckCredentialsAsync(userDto);
            User user =  await _loginService.GetUserByEmailAsync(userDto.Email);
            string token = _jwtValidationService.GenerateToken(user);
            return Ok(token);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto userRequest)
        {
            await _loginService.CreateUserAsync(userRequest);   
            return Ok(new { message = "Gebruiker toegevoegd." });
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            await _loginService.UpdateUserAsync(updateUserRequestDto);   
            return Ok(new { message = "Gebruiker geupdate." });
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            await _loginService.DeleteUserAsync(email);   
            return Ok(new { message = "Gebruiker verwijderd." });
        }
    }
}