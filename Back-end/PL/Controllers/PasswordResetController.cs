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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _passwordResetService.PostResetCode(email);
            return Ok(new { message = "Mail gestuurd." });
        }

        [AllowAnonymous]
        [HttpGet("check-code")]
        public async Task<IActionResult> CheckEnteredCode(string email, string code)
        {
            await _passwordResetService.CheckEnteredCode(email, code);
            return Ok(new { message = "Mail gestuurd." });
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddPassword(PasswordChangeRequest request)
        {
            Console.WriteLine("hoihoi");
            await _passwordResetService.PostPassword(request.Email, request.Password1, request.Password2, request.Code);
            return Ok(new { message = "Wachtwoord aangepast." });
        }
    }
}