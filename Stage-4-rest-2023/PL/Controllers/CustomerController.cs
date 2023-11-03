using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("customer")]
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

        [HttpGet]
        public IActionResult GetPagedCustomers(string? searchfield, int page, int pageSize)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var (pagedCustomers, pager) = customerService.GetPagedCustomers(searchfield, page, pageSize);

                var response = new
                {
                    Customers = pagedCustomers,
                    Pager = new
                    {
                        pager.TotalItems,
                        pager.CurrentPage,
                        pager.PageSize,
                        pager.TotalPages,
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Filters and retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchField">The search field used to filter customers.</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        [HttpGet("filter")]
        public IActionResult GetFilteredCustomers(string? searchField)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var customers = customerService.GetFilteredCustomers(searchField);
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
        public IActionResult GetCustomerById(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var customer = customerService.GetCustomerById(id);
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
        public IActionResult PostCustomer(Customer cus)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                int id = customerService.PostCustomer(cus);
                return Ok(id);
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
        public IActionResult PutCustomer(Customer customer)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                customerService.PutCustomer(customer);
                return Ok(new { message = "Customer updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                customerService.DeleteCustomer(id);
                return Ok(new { message = "Customer deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}