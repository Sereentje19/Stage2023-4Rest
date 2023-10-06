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

        /// <summary>
        /// Initializes a new instance of the CustomerController class.
        /// </summary>
        /// <param name="cs">The customer service for managing customers.</param>
        /// <param name="jwt">The JWT validation service for token validation.</param>
        public CustomerController(ICustomerService cs, IJwtValidationService jwt)
        {
            customerService = cs;
            jwtValidationService = jwt;
        }

        /// <summary>
        /// Filters and retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchField">The search field used to filter customers.</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        [HttpGet("Filter")]
        public IActionResult FilterAll(string searchField)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var customers = customerService.FilterAll(searchField);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a customer by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, an error message.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var customer = customerService.GetById(id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new customer to the system.
        /// </summary>
        /// <param name="cus">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer if successful; otherwise, an error message.</returns>
        [HttpPost]
        public IActionResult Post(Customer cus)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                int result = customerService.Post(cus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <param name="cus">The document entity to be updated.</param>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public IActionResult Put(Customer cus)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                customerService.Put(cus);
                return Ok(new { message = "Customer updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}