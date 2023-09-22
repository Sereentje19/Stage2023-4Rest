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

        public CustomerController(ICustomerService cs)
        {
            customerService = cs;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = customerService.GetAll();
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPost]
        public int Post(Customer cus)
        {
           return customerService.Post(cus);
        }

    }
}