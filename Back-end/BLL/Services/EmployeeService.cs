using Azure;
using DAL.Repositories;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Retrieves a paged list of customers based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of customers per page.</param>
        /// <returns>A tuple containing paged customers and pagination information.</returns>
        public async Task<(IEnumerable<object>, Pager)> GetPagedEmployee(string searchfield, int page, int pageSize)
        {
            var (pagedCustomers, numberOfcustomers) = await _employeeRepository.GetAllEmployee(searchfield, page, pageSize);
            Pager pager = new Pager(numberOfcustomers, page, pageSize);
            return (pagedCustomers, pager);
        }
        
        public async Task<(IEnumerable<object>, Pager)> GetPagedArchivedEmployee(string searchfield, int page, int pageSize)
        {
            var (pagedCustomers, numberOfcustomers) = await _employeeRepository.GetAllArchivedEmployees(searchfield, page, pageSize);
            Pager pager = new Pager(numberOfcustomers, page, pageSize);
            return (pagedCustomers, pager);
        }

        /// <summary>
        /// Retrieves a filtered list of customers based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <returns>A collection of matching Customer objects.</returns>
        public async Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield)
        {
            return await _employeeRepository.GetFilteredEmployee(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _employeeRepository.GetEmployeeById(id);
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="employee">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer.</returns>
        public async Task<int> PostEmployee(Employee employee)
        {
            return await _employeeRepository.AddEmployee(employee);
        }
        
        public async Task PutEmployeeIsArchived(Employee employee)
        {
            await _employeeRepository.UpdateEmployeeIsArchived(employee);
        }

        /// <summary>
        /// Updates an existing customer's information.
        /// </summary>
        /// <param name="employee">The Customer object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PutEmployee(Employee employee)
        {
            await _employeeRepository.UpdateEmployee(employee);
        }

        /// <summary>
        /// Deletes a customer based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployee(id);
        }
    }
}
