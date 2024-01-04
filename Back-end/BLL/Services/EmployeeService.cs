using Azure;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services.Authentication;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.Graph;
using Microsoft.Graph.Models;

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
        public async Task<(IEnumerable<object>, Pager)> GetPagedEmployees(string searchfield, int page, int pageSize)
        {
            (IEnumerable<object> pagedCustomers, int numberOfcustomers) = await _employeeRepository.GetAllEmployees(searchfield, page, pageSize);
            Pager pager = new Pager(numberOfcustomers, page, pageSize);
            return (pagedCustomers, pager);
        }
        
        /// <summary>
        /// Retrieves a paged list of archived employees based on the specified search criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering employees.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A tuple containing:
        ///   1. An enumerable collection of archived employee objects.
        ///   2. A Pager object providing information about the pagination.
        /// </returns>
        public async Task<(IEnumerable<object>, Pager)> GetPagedArchivedEmployees(string searchfield, int page, int pageSize)
        {
            (IEnumerable<object> pagedCustomers, int numberOfcustomers) = await _employeeRepository.GetAllArchivedEmployees(searchfield, page, pageSize);
            Pager pager = new Pager(numberOfcustomers, page, pageSize);
            return (pagedCustomers, pager);
        }

        /// <summary>
        /// Retrieves a filtered list of customers based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <returns>A collection of matching Customer objects.</returns>
        public Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield)
        {
            return _employeeRepository.GetFilteredEmployeesAsync(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return _employeeRepository.GetEmployeeByIdAsync(id);
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="employeeRequest">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer.</returns>
        public Task<int> CreateEmployeeAsync(EmployeeRequestDto employeeRequest)
        {
            ValidationHelper.ValidateObject(employeeRequest);
            return _employeeRepository.CreateEmployeeAsync(employeeRequest);
        }
        
        /// <summary>
        /// Updates the 'IsArchived' status of an employee in the system.
        /// </summary>
        /// <param name="employeeRequest">The Employee object containing updated information.</param>
        /// <returns>
        /// A task representing the asynchronous operation of updating the 'IsArchived' status.
        /// </returns>
        public Task UpdateEmployeeIsArchivedAsync(EmployeeRequestDto employeeRequest)
        {
            ValidationHelper.ValidateObject(employeeRequest);
            return _employeeRepository.UpdateEmployeeIsArchivedAsync(employeeRequest);
        }

        /// <summary>
        /// Updates an existing customer's information.
        /// </summary>
        /// <param name="employeeRequest">The Customer object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task UpdateEmployeeAsync(EmployeeRequestDto employeeRequest)
        {
            ValidationHelper.ValidateObject(employeeRequest);
            return _employeeRepository.UpdateEmployeeAsync(employeeRequest);
        }

        /// <summary>
        /// Deletes a customer based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task DeleteEmployeeAsync(int id)
        {
            return _employeeRepository.DeleteEmployeeAsync(id);
        }
    }
}
