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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IJwtValidationService jwtValidationService;

        public CustomerController(ICustomerService cs, IJwtValidationService jwtv)
        {
            customerService = cs;
            jwtValidationService = jwtv;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            jwtValidationService.ValidateToken(HttpContext);
            var customer = customerService.GetAll();
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            jwtValidationService.ValidateToken(HttpContext);
            var customer = customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPost]
        public int Post(Customer cus)
        {
            jwtValidationService.ValidateToken(HttpContext);
           return customerService.Post(cus);
        }

    }
}