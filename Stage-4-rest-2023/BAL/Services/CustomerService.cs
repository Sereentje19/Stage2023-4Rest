using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Repositories;

namespace Stage4rest2023.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Retrieves a paged list of customers based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of customers per page.</param>
        /// <returns>A tuple containing paged customers and pagination information.</returns>
        public async Task<(IEnumerable<object>, Pager)> GetPagedCustomers(string searchfield, int page, int pageSize)
        {
            var (pagedCustomers, numberOfcustomers) = await _customerRepository.GetAllCustomers(searchfield, page, pageSize);
            Pager pager = new Pager(numberOfcustomers, page, pageSize);
            return (pagedCustomers, pager);
        }

        /// <summary>
        /// Retrieves a filtered list of customers based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <returns>A collection of matching Customer objects.</returns>
        public async Task<IEnumerable<Customer>> GetFilteredCustomers(string searchfield)
        {
            return await _customerRepository.GetFilteredCustomers(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public async Task<Customer> GetCustomerById(int id)
        {
            return await _customerRepository.GetCustomerById(id);
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer.</returns>
        public async Task<int> PostCustomer(Customer customer)
        {
            return await _customerRepository.AddCustomer(customer);
        }

        /// <summary>
        /// Updates an existing customer's information.
        /// </summary>
        /// <param name="customer">The Customer object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PutCustomer(Customer customer)
        {
            await _customerRepository.UpdateCustomer(customer);
        }
        
        /// <summary>
        /// Deletes a customer based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteCustomer(int id)
        {
            await _customerRepository.DeleteCustomer(id);
        }
    }
}