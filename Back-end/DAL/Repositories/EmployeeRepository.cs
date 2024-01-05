using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.Graph;

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
        private IQueryable<Employee> QueryGetEmployees(string searchfield)
        {
            return _context.Employees
                .Where(employee => string.IsNullOrEmpty(searchfield) ||
                                   employee.Name.Contains(searchfield) ||
                                   employee.Email.Contains(searchfield))
                .OrderBy(employee => employee.Name);
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
            Expression<Func<Employee, bool>> filter)
        {
            int skipCount = page * pageSize;
            
            if (skipCount > 0)
            {
                skipCount -= 5;
            }

            IQueryable<Employee> query = QueryGetEmployees(searchfield).Where(filter);
            int numberOfEmployees = query.Count();

            IEnumerable<Employee> employeeList = query
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
        public Task<(IEnumerable<object>, int)> GetAllArchivedEmployees(string searchfield, int page,
            int pageSize)
        {
            return Task.FromResult(GetPagedEmployeesInternal(searchfield, page, pageSize, item => item.IsArchived));
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
        public Task<(IEnumerable<object>, int)> GetAllEmployees(string searchfield, int page,
            int pageSize)
        {
            return Task.FromResult(GetPagedEmployeesInternal(searchfield, page, pageSize, item => !item.IsArchived));
        }


        /// <summary>
        /// Retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <returns>A collection of customers filtered by the provided search field.</returns>
        public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield)
        {
            IQueryable<Employee> employee = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchfield))
            {
                searchfield = searchfield.ToLower();
                employee = employee.Where(emp =>
                    emp.Name.ToLower().Contains(searchfield) ||
                    emp.Email.ToLower().Contains(searchfield)
                );
            }

            return await employee.OrderBy(customer => customer.Name).ToListAsync();
        }


        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="employeeRequest">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer, or the ID of an existing customer if the same name and email combination is found.</returns>
        /// <exception cref="Exception">Thrown when the customer's name or email is empty.</exception>
        public async Task<int> CreateEmployeeAsync(Employee employeeRequest)
        {
            //check if the customer already exist, then don't add it again.
            Employee existingEmployee =
                await _dbSet.FirstOrDefaultAsync(c => c.Email == employeeRequest.Email);

            if (existingEmployee != null && existingEmployee.Name != employeeRequest.Name)
            {
                throw new InputValidationException("Email bestaat al.");
            }
            
            await _dbSet.AddAsync(employeeRequest);
            await _context.SaveChangesAsync();
            return employeeRequest.EmployeeId;
        }
        
        /// <summary>
        /// Updates the IsArchived property of an existing employee.
        /// </summary>
        /// <param name="employeeRequest">The EmployeeRequestDto containing the EmployeeId and the new IsArchived value.</param>
        /// <exception cref="NotFoundException">Thrown if no employee is found with the specified EmployeeId.</exception>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateEmployeeIsArchivedAsync(EmployeeRequestDto employeeRequest)
        {
            Employee existingEmployee = await _dbSet.FindAsync(employeeRequest.EmployeeId);

            if (existingEmployee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }

            existingEmployee.IsArchived = employeeRequest.IsArchived;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="employeeRequest">The document entity to be updated.</param>
        public async Task UpdateEmployeeAsync(EmployeeRequestDto employeeRequest)
        {
            Employee existingEmployee = await _dbSet.FindAsync(employeeRequest.EmployeeId);

            if (existingEmployee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }

            await CheckEmailExistsAsync(employeeRequest);
            _context.Entry(existingEmployee).CurrentValues.SetValues(employeeRequest);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a customer and associated loan history and documents based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteEmployeeAsync(int id)
        {
            Employee employee = await _dbSet.FindAsync(id);

            if (employee == null)
            {
                throw new NotFoundException("Geen medewerker gevonden");
            }

            _dbSet.Remove(employee);
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Checks if an employee with the specified email already exists, excluding the provided EmployeeId.
        /// </summary>
        /// <param name="employeeRequest">The EmployeeRequestDto containing the email to check.</param>
        /// <exception cref="InputValidationException">Thrown if the email already exists for a different employee.</exception>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task CheckEmailExistsAsync(EmployeeRequestDto employeeRequest)
        {
            Employee existingEmployee =
                await _dbSet.FirstOrDefaultAsync(c =>
                    c.Email == employeeRequest.Email && c.EmployeeId != employeeRequest.EmployeeId);

            if (existingEmployee != null)
            {
                throw new InputValidationException("Email bestaat al.");
            }
        }

    }
}