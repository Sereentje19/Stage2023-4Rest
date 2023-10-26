using System.Text.Json;
using Back_end.Exceptions;
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


        public List<Customer> GetAll(string searchfield)
        {
            return _context.Customers
                .Where(customer => string.IsNullOrEmpty(searchfield) ||
                                  customer.Name.Contains(searchfield) ||
                                  customer.Email.Contains(searchfield))
                .OrderBy(customer => customer.Name)
                .ToList(); 
        }


        public IEnumerable<Product> GetAll(string searchfield, ProductType? dropdown)
        {
            IQueryable<Product> query = from product in _context.Products
                                        where (string.IsNullOrEmpty(searchfield) ||
                                            product.SerialNumber.Contains(searchfield) ||
                                            product.ExpirationDate.ToString().Contains(searchfield) ||
                                            product.PurchaseDate.ToString().Contains(searchfield))
                                        && (dropdown == ProductType.Not_Selected || product.Type == dropdown)
                                        select product;

            var productList = query.ToList();
            return productList;
        }


        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by (can match customer name, email, or ID).</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        public IEnumerable<Customer> GetFilteredCustomers(string searchfield)
        {
            if (string.IsNullOrWhiteSpace(searchfield))
            {
                return new List<Customer>();
            }

            return _dbSet
            .Where(customer =>
                customer.Name.Contains(searchfield) ||
                customer.Email.Contains(searchfield)
            )
            .ToList();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public Customer GetById(int id)
        {
            return _dbSet.Find(id);
        }


        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer, or the ID of an existing customer if the same name and email combination is found.</returns>
        /// <exception cref="Exception">Thrown when the customer's name or email is empty.</exception>
        public int Add(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Email))
            {
                throw new CustomerAddException("Klant naam of email is leeg.");
            }

            //if the customer already exist, don't add it again.
            var existingCustomer = _dbSet.FirstOrDefault(c => c.Name == customer.Name && c.Email == customer.Email);

            if (existingCustomer != null)
            {
                return existingCustomer.CustomerId;
            }

            _dbSet.Add(customer);
            _context.SaveChanges();
            return customer.CustomerId;
        }

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="customer">The document entity to be updated.</param>
        public void Update(Customer customer)
        {
            try
            {
                _dbSet.Update(customer);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Er is een conflict opgetreden bij het bijwerken van de klantgegevens.");
            }
        }

        public void Delete(Customer customer)
        {
            try
            {
                Document doc = _context.Documents.Find(customer.CustomerId);
                doc.Customer = null;

                _dbSet.Remove(customer);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Er is een conflict opgetreden bij het bijwerken van de klantgegevens.");
            }
        }
    }
}