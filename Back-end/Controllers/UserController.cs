using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService us)
        {
            userService = us;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = userService.GetAll();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = userService.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            userService.Post(user);
            return Ok(new { message = "User created" });
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            userService.Put(user);
            return Ok(new { message = "User updated" });
        }

        [HttpDelete]
        public IActionResult Delete(User user)
        {
            userService.Delete(user);
            return Ok(new { message = "User Deleted" });
        }
    }
}