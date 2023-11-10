using Stage4rest2023.Models;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("customer")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        /// <summary>
        /// Retrieves a paged list of customers based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of customers per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged customers and pagination details.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetPagedCustomers(string? searchfield, int page, int pageSize)
        {
            var (pagedCustomers, pager) = await _customerService.GetPagedCustomers(searchfield, page, pageSize);

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

        /// <summary>
        /// Filters and retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchField">The search field used to filter customers.</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredCustomers(string? searchField)
        {
            IEnumerable<Customer> customers = await _customerService.GetFilteredCustomers(searchField);
            return Ok(customers);
        }

        /// <summary>
        /// Retrieves a customer by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, an error message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            Customer customer = await _customerService.GetCustomerById(id);
            return Ok(customer);
        }

        /// <summary>
        /// Adds a new customer to the system.
        /// </summary>
        /// <param name="cus">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer if successful; otherwise, an error message.</returns>
        [HttpPost]
        public  async Task<IActionResult>  PostCustomer(Customer cus)
        {
            int id = await _customerService.PostCustomer(cus);
            return Ok(id);
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <param name="cus">The document entity to be updated.</param>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public async Task<IActionResult> PutCustomer(Customer customer)
        {
            await _customerService.PutCustomer(customer);
            return Ok(new { message = "Customer updated" });
        }

        /// <summary>
        /// Deletes a customer based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok(new { message = "Customer deleted" });
        }
    }
}