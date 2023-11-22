﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using BLL.Services;
using PL.Models;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("forgot-password")]
    [Authorize]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _passwordResetService.PostResetCode(email);
            return Ok(new { message = "Mail gestuurd" });
        }

        [AllowAnonymous]
        [HttpGet("check-code")]
        public async Task<IActionResult> CheckEnteredCode(string email, string code)
        {
            await _passwordResetService.CheckEnteredCode(email, code);
            return Ok(new { message = "Mail gestuurd" });
        }
    }
}