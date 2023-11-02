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
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAll(string searchfield)
        {
            return _customerRepository.GetAll(searchfield);
        }

        public (IEnumerable<object>, Pager) GetAllPagedCustomers(string searchfield, int page, int pageSize)
        {
            var customers = _customerRepository.GetAll(searchfield);

            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(customers.Count(), page, pageSize);

            var pagedCustomers = customers
                .Skip(skipCount)
                .Take(pageSize)
                .Select(cus => new
                {
                    cus.Name,
                    cus.Email,
                    cus.CustomerId
                })
                .ToList();

            return (pagedCustomers.Cast<object>(), pager);
        }


        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by.</param>
        /// <returns>A collection of customers matching the search criteria or an empty list if the searchfield is null or whitespace.</returns>
        public IEnumerable<Customer> GetFilteredCustomers(string searchfield)
        {
            return _customerRepository.GetFilteredCustomers(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public Customer GetById(int id)
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

        public void Put(Customer customer)
        {
            _customerRepository.Update(customer);
        }
        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }
    }
}