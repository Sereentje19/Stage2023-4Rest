using Back_end.Models;
using Back_end.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Customer> _dbSet;

        /// <summary>
        /// Initializes a new instance of the CustomerRepository class with the provided NotificationContext.
        /// </summary>
        /// <param name="context">The NotificationContext used for database interactions.</param>
        public CustomerRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Customer>();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public CustomerDTO GetById(int id)
        {
            Customer cus = _dbSet.Find(id);

            var customerDTO = new CustomerDTO
            {
                Email = cus.Email,
                Name = cus.Name
            };

            return customerDTO;
        }

        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by (can match customer name, email, or ID).</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        public IEnumerable<Customer> FilterAll(string searchfield)
        {
            var filteredCustomers = _dbSet
            .Where(customer =>
                customer.Name.Contains(searchfield) ||
                customer.Email.Contains(searchfield) ||
                customer.CustomerId.ToString().Contains(searchfield)
            )
            .ToList();

            return filteredCustomers;
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="entity">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer, or the ID of an existing customer if the same name and email combination is found.</returns>
        /// <exception cref="Exception">Thrown when the customer's name or email is empty.</exception>
        public int Add(Customer entity)
        {
            if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.Email))
            {
                throw new Exception("Klant naam of email is leeg.");
            }

            var existingCustomer = _dbSet.FirstOrDefault(c => c.Name == entity.Name && c.Email == entity.Email);

            if (existingCustomer != null)
            {
                return existingCustomer.CustomerId;
            }

            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity.CustomerId;
        }

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="entity">The document entity to be updated.</param>
        public void Update(CustomerDTO entity)
        {
            if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.Email))
            {
                throw new Exception("Klant naam of email is leeg.");
            }

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}