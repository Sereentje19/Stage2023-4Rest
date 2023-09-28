using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Back_end.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using System.Security.Principal;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IJwtValidationService jwtValidationService;
        public LoginController(ILoginService lo, IJwtValidationService jwtv)
        {
            loginService = lo;
            jwtValidationService = jwtv;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {
            try
            {
                User currentUser = loginService.checkCredentials(user);

                if (currentUser != null)
                {
                    var token = jwtValidationService.GenerateToken();
                    return Ok(token);
                }
                return NotFound("Email of wachtwoord incorrect");
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}