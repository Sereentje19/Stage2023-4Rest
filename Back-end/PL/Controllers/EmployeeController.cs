﻿using BLL.Interfaces;
using BLL.Services;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("employee")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        

        /// <summary>
        /// Retrieves a paged list of customers based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of customers per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged customers and pagination details.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetPagedEmployees(string searchfield, int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedEmployees, Pager pager) = await _employeeService.GetPagedEmployees(searchfield, page, pageSize);

            var response = new
            {
                Employees = pagedEmployees,
                Pager = new
                {
                    pager.TotalItems,
                    pager.CurrentPage,
                    pager.PageSize,
                    pager.TotalPages,
                }
            };

            return Ok(response);
        }
        
        /// <summary>
        /// Retrieves a paged list of archived employees.
        /// </summary>
        /// <param name="searchfield">Optional. The search criteria for filtering archived employees.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// a response object containing a paged list of archived employees and pagination information.
        /// </returns>
        [HttpGet("archive")]
        public async Task<IActionResult> GetPagedArchivedEmployees(string searchfield, int page, int pageSize)
        {
            (IEnumerable<object> pagedEmployees, Pager pager) = await _employeeService.GetPagedArchivedEmployees(searchfield, page, pageSize);

            var response = new
            {
                Employees = pagedEmployees,
                Pager = new
                {
                    pager.TotalItems,
                    pager.CurrentPage,
                    pager.PageSize,
                    pager.TotalPages,
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Filters and retrieves a list of customers based on a search field.
        /// </summary>
        /// <param name="searchField">The search field used to filter customers.</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEmployeesAsync(string searchField)
        {
            IEnumerable<Employee> customers = await _employeeService.GetFilteredEmployeesAsync(searchField);
            return Ok(customers);
        }

        /// <summary>
        /// Retrieves a customer by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, an error message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(int id)
        {
            Employee employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        /// <summary>
        /// Adds a new customer to the system.
        /// </summary>
        /// <param name="cus">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer if successful; otherwise, an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(EmployeeRequestDto cus)
        {
            int id = await _employeeService.CreateEmployeeAsync(cus);
            return Ok(id);
        }

        /// <summary>
        /// Updates the archived status of an employee.
        /// </summary>
        /// <param name="doc">The Employee object containing information about the employee to be updated.</param>
        /// <returns>
        /// a message indicating that the employee's archived status has been updated.
        /// </returns>
        [HttpPut("archive")]
        public async Task<IActionResult> UpdateIsArchivedAsync(EmployeeRequestDto doc)
        {
            await _employeeService.UpdateEmployeeIsArchivedAsync(doc);
            return Ok(new { message = "Medewerker geupdate." });
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeAsync(EmployeeRequestDto employeeRequest)
        {
            await _employeeService.UpdateEmployeeAsync(employeeRequest);
            return Ok(new { message = "Medewerker geupdate. " });
        }

        /// <summary>
        /// Deletes a customer based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok(new { message = "Medewerker verwijderd." });
        }
    }
}
