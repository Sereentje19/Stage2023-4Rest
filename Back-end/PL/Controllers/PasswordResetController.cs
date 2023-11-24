using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using BLL.Services;
using PL.Models;
using PL.Models.Requests;

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

        /// <summary>
        /// Initiates the password reset process by sending a reset code via email.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>
        /// a success message indicating that an email has been sent.
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _passwordResetService.PostResetCode(email);
            return Ok(new { message = "Mail gestuurd." });
        }

        /// <summary>
        /// Checks the validity of the entered reset code during the password reset process.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="code">The reset code entered by the user.</param>
        /// <returns>
        /// a success message indicating that the code is valid.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("check-code")]
        public async Task<IActionResult> CheckEnteredCode(string email, string code)
        {
            await _passwordResetService.CheckEnteredCode(email, code);
            return Ok(new { message = "Mail gestuurd." });
        }
        
        /// <summary>
        /// Adds a new password for a user during the password reset process.
        /// </summary>
        /// <param name="request">A PasswordChangeRequest object containing the new password and associated information.</param>
        /// <returns>
        /// a success message indicating that the password has been updated.
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddPassword(PasswordChangeRequest request)
        {
            await _passwordResetService.PostPassword(request);
            return Ok(new { message = "Wachtwoord aangepast." });
        }
    }
}