using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Employee> _dbSet;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Employee>();
        }

        /// <summary>
        /// Retrieves a paged list of customers based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of customers per page.</param>
        /// <returns>
        /// A tuple containing a collection of customers and the total number of customers.
        /// </returns>
        public async Task<(IEnumerable<object>, int)> GetAllEmployee(string searchfield, int page, int pageSize)
        {
            IQueryable<Employee> query = _context.Employees
                .Where(customer => string.IsNullOrEmpty(searchfield) ||
                                   customer.Name.Contains(searchfield) ||
                                   customer.Email.Contains(searchfield))
                .OrderBy(customer => customer.Name);

            int numberOfCustomers = await query.CountAsync();
            int skipCount = Math.Max(0, (page - 1) * pageSize);

            IEnumerable<Employee> customerList = await query
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();

            return (customerList, numberOfCustomers);
        }

        /// <summary>
        /// Retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <returns>A collection of customers filtered by the provided search field.</returns>
        public async Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield)
        {
            IQueryable<Employee> customers = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchfield))
            {
                customers.Where(customer =>
                    customer.Name.Contains(searchfield) ||
                    customer.Email.Contains(searchfield)
                );
            }

            return await customers.OrderBy(customer => customer.Name).ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="employee">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer, or the ID of an existing customer if the same name and email combination is found.</returns>
        /// <exception cref="Exception">Thrown when the customer's name or email is empty.</exception>
        public async Task<int> AddEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name) || string.IsNullOrWhiteSpace(employee.Email))
            {
                throw new InputValidationException("Klant naam of email is leeg.");
            }

            //if the customer already exist, don't add it again.
            Employee existingEmployee =
                await _dbSet.FirstOrDefaultAsync(c => c.Name == employee.Name && c.Email == employee.Email);

            if (existingEmployee == null)
            {
                await _dbSet.AddAsync(employee);
                await _context.SaveChangesAsync();
            }

            return employee.EmployeeId;
        }

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="employee">The document entity to be updated.</param>
        public async Task UpdateEmployee(Employee employee)
        {
            _dbSet.Update(employee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a customer and associated loan history and documents based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteEmployee(int id)
        {
            List<LoanHistory> loans = _context.LoanHistory.Where(l => l.Employee.EmployeeId == id).ToList();
            foreach (LoanHistory loan in loans)
            {
                _context.LoanHistory.Remove(loan);
            }

            List<Document> docs = _context.Documents.Where(l => l.Employee.EmployeeId == id).ToList();
            foreach (Document doc in docs)
            {
                _context.Documents.Remove(doc);
            }

            _dbSet.Remove(_dbSet.Find(id));
            await _context.SaveChangesAsync();
        }
    }
}
