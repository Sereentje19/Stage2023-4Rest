using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
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
        public IActionResult Post(Customer cus)
        {
            customerService.Post(cus);
            return Ok(new { message = "Customer created" });
        }

    }
}