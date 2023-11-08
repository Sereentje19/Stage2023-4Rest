using System.Text.Json;
using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Stage4rest2023.Repositories
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

        public (IEnumerable<object>, int) GetAllCustomers(string searchfield, int page, int pageSize)
        {
            IQueryable<Customer> query = _context.Customers
                .Where(customer => string.IsNullOrEmpty(searchfield) ||
                                   customer.Name.Contains(searchfield) ||
                                   customer.Email.Contains(searchfield))
                .OrderBy(customer => customer.Name);
            
            int numberOfCustomers = query.Count();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            
            IEnumerable<Customer> customerList = query
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (customerList, numberOfCustomers);
        }

        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by (can match customer name, email, or ID).</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        public IEnumerable<Customer> GetFilteredCustomers(string searchfield)
        {
            IQueryable<Customer> customers = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchfield))
            {
                customers.Where(customer =>
                    customer.Name.Contains(searchfield) ||
                    customer.Email.Contains(searchfield)
                );
            }

            return customers.OrderBy(customer => customer.Name).ToList();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public Customer GetCustomerById(int id)
        {
            Customer customer = _dbSet.Find(id);

            if (customer == null)
            {
                throw new ItemNotFoundException("geen medewerker gevonden.");
            }

            return customer;
        }


        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer, or the ID of an existing customer if the same name and email combination is found.</returns>
        /// <exception cref="Exception">Thrown when the customer's name or email is empty.</exception>
        public int AddCustomer(Customer customer)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customer.Name) || string.IsNullOrWhiteSpace(customer.Email))
                {
                    throw new InputValidationException("Klant naam of email is leeg.");
                }

                //if the customer already exist, don't add it again.
                var existingCustomer = _dbSet.FirstOrDefault(c => c.Name == customer.Name && c.Email == customer.Email);

                if (existingCustomer == null)
                {
                    _dbSet.Add(customer);
                    _context.SaveChanges();
                }

                return customer.CustomerId;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="customer">The document entity to be updated.</param>
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                _dbSet.Update(customer);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void DeleteCustomer(int id)
        {
            try
            {
                List<LoanHistory> loans = _context.LoanHistory.Where(l => l.Customer.CustomerId == id).ToList();
                foreach (var loan in loans)
                {
                    _context.LoanHistory.Remove(loan);
                }

                List<Document> docs = _context.Documents.Where(l => l.Customer.CustomerId == id).ToList();
                foreach (var doc in docs)
                {
                    _context.Documents.Remove(doc);
                }

                _dbSet.Remove(_dbSet.Find(id));
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }
    }
}