using Back_end.Models;
using Back_end.Models.DTOs;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Initializes a new instance of the CustomerService class with the provided CustomerRepository.
        /// </summary>
        /// <param name="cr">The CustomerRepository used for customer-related operations.</param>
        public CustomerService(ICustomerRepository cr)
        {
            _customerRepository = cr;
        }

        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by.</param>
        /// <returns>A collection of customers matching the search criteria or an empty list if the searchfield is null or whitespace.</returns>
        public IEnumerable<Customer> FilterAll(string searchfield)
        {
            if (string.IsNullOrWhiteSpace(searchfield))
            {
                return new List<Customer>();
            }
            return _customerRepository.FilterAll(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public CustomerDTO GetById(int id)
        {
            return _customerRepository.GetById(id);
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer.</returns>
        public int Post(Customer customer)
        {
            return _customerRepository.Add(customer);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="customer">The document entity to be updated.</param>
        public void Put(Customer customer)
        {
            _customerRepository.Update(customer);
        }

    }
}