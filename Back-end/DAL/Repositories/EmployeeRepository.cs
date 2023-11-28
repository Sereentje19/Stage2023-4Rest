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
        /// Queries the database for employees based on a search field.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering employees.</param>
        /// <returns>
        /// An IQueryable<Employee> representing the filtered employees based on the search criteria.
        /// </returns>
        public IQueryable<Employee> QueryGetEmployees(string searchfield)
        {
            return _context.Employees
                .Where(customer => string.IsNullOrEmpty(searchfield) ||
                                   customer.Name.Contains(searchfield) ||
                                   customer.Email.Contains(searchfield))
                .OrderBy(customer => customer.Name);
        }

        /// <summary>
        /// Retrieves a paged list of employees based on search criteria and a filter function.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering employees.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="filter">A filter function to determine which employees to include in the result.</param>
        /// <returns>
        /// A tuple containing:
        ///   1. An IEnumerable<object> representing the paged list of employees.
        ///   2. An int representing the total number of employees matching the search criteria and filter.
        /// </returns>
        private (IEnumerable<object>, int) GetPagedEmployeesInternal(string searchfield, int page, int pageSize,
            Func<Employee, bool> filter)
        {
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            IQueryable<Employee> query = QueryGetEmployees(searchfield);
            int numberOfEmployees = query.Where(filter).Count();

            IEnumerable<Employee> employeeList = query
                .Where(filter)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (employeeList, numberOfEmployees);
        }

        /// <summary>
        /// Retrieves a paged list of archived employees based on search criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering archived employees.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A tuple containing:
        ///   1. An IEnumerable<object> representing the paged list of archived employees.
        ///   2. An int representing the total number of archived employees matching the search criteria.
        /// </returns>
        public async Task<(IEnumerable<object>, int)> GetAllArchivedEmployees(string searchfield, int page,
            int pageSize)
        {
            return GetPagedEmployeesInternal(searchfield, page, pageSize, item => item.IsArchived);
        }

        /// <summary>
        /// Retrieves a paged list of active employees based on search criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering active employees.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A tuple containing:
        ///   1. An IEnumerable<object> representing the paged list of active employees.
        ///   2. An int representing the total number of active employees matching the search criteria.
        /// </returns>
        public async Task<(IEnumerable<object>, int)> GetAllEmployee(string searchfield, int page,
            int pageSize)
        {
            return GetPagedEmployeesInternal(searchfield, page, pageSize, item => !item.IsArchived);
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
                searchfield = searchfield.ToLower();
                customers = customers.Where(customer =>
                    customer.Name.ToLower().Contains(searchfield) ||
                    customer.Email.ToLower().Contains(searchfield)
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
            if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                throw new InputValidationException("Klant naam is leeg.");
            }

            //check if the customer already exist, don't add it again.
            Employee existingEmployee =
                await _dbSet.FirstOrDefaultAsync(c => c.Email == employee.Email);

            if (existingEmployee == null)
            {
                await _dbSet.AddAsync(employee);
                await _context.SaveChangesAsync();
            }

            return employee.EmployeeId;
        }

        public async Task UpdateEmployeeIsArchived(Employee employee)
        {
            Employee existingEmployee = await _dbSet.FindAsync(employee.EmployeeId);
            
            if (existingEmployee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }
            
            existingEmployee.IsArchived = employee.IsArchived;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="employee">The document entity to be updated.</param>
        public async Task UpdateEmployee(Employee employee)
        {
            Employee existingEmployee = await _dbSet.FindAsync(employee.EmployeeId);
            
            if (existingEmployee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }

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
            Employee employee = await _dbSet.FindAsync(id);
            
            if (employee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }

            _dbSet.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}