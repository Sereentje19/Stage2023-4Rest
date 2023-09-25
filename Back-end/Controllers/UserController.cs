using System;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtValidationService jwtValidationService;

        public UserController(IUserService us, IJwtValidationService jwtv)
        {
            userService = us;
            jwtValidationService = jwtv;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            jwtValidationService.ValidateToken(HttpContext);
            var user = userService.GetAll();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            jwtValidationService.ValidateToken(HttpContext);
            var user = userService.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            jwtValidationService.ValidateToken(HttpContext);
            userService.Post(user);
            return Ok(new { message = "User created" });
        }
    }
}