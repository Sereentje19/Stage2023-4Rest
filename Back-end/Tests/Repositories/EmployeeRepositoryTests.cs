﻿using BLL.Services;
using DAL.Data;
using DAL.Exceptions;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private readonly List<Employee> _employees = new()
        {
            new Employee
            {
                EmployeeId = 1, Name = "John", Email = "john@example.com", IsArchived = false
            },
            new Employee
            {
                EmployeeId = 2, Name = "Meredith", Email = "Meredith@example.com", IsArchived = true
            },
            new Employee
            {
                EmployeeId = 3, Name = "Regina", Email = "Regina@example.com", IsArchived = true
            },
            new Employee
            {
                EmployeeId = 4, Name = "Jane", Email = "jane@example.com", IsArchived = false
            },
            new Employee
            {
                EmployeeId = 5, Name = "Jane", Email = "jane@example.com", IsArchived = false
            },
        };

        private static EmployeeRequestDto MapEmployeeToDto(Employee emp)
        {
            return new EmployeeRequestDto()
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Email = emp.Email,
                IsArchived = emp.IsArchived
            };
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllArchivedEmployees_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                (IEnumerable<object>, int) nonExistentResult = await repository.GetAllArchivedEmployees("Doe", 1, 10);
                Assert.Empty(nonExistentResult.Item1);
                Assert.Equal(0, nonExistentResult.Item2);

                (IEnumerable<object>, int) result = await repository.GetAllArchivedEmployees("Regina", 1, 10);
                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);

                (IEnumerable<object>, int) pageTwoResult = await repository.GetAllArchivedEmployees("", 1, 5);
                Assert.Equal(2, pageTwoResult.Item2);
            }
        }

        [Fact]
        public async Task GetAllEmployee_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                context.Employees.AddRange(_employees);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                (IEnumerable<object>, int) nonExistentResult = await repository.GetAllEmployees("Doe", 1, 10);
                Assert.Empty(nonExistentResult.Item1);
                Assert.Equal(0, nonExistentResult.Item2);

                (IEnumerable<object>, int) result = await repository.GetAllEmployees("John", 1, 10);
                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);

                (IEnumerable<object>, int) pageTwoResult = await repository.GetAllEmployees("jane", 2, 1);
                Assert.Equal(2, pageTwoResult.Item2);
            }
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldReturnFilteredResults()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);
                IEnumerable<Employee> result = await repository.GetFilteredEmployeesAsync("Regina");

                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal("Regina", result.First().Name);
            }
        }

        [Fact]
        public async Task GetFilteredEmployee_WithNoSearchField_ShouldReturnAllEmployees()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);
                IEnumerable<Employee> result = await repository.GetFilteredEmployeesAsync("");

                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int employeeId = 1;
                Employee employee = new Employee
                    { EmployeeId = employeeId, Name = "John", Email = "john@example.com", IsArchived = false };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);
                Employee result = await repository.GetEmployeeByIdAsync(employeeId);

                Assert.NotNull(result);
                Assert.Equal(employeeId, result.EmployeeId);
                Assert.Equal("John", result.Name);
                Assert.Equal("john@example.com", result.Email);
                Assert.False(result.IsArchived);
            }
        }

        [Fact]
        public async Task GetEmployeeById_WithNonExistentId_ShouldReturnNull()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int nonExistentId = 999;

                EmployeeRepository repository = new EmployeeRepository(context);
                Employee result = await repository.GetEmployeeByIdAsync(nonExistentId);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddEmployee_WithValidEmployee_ShouldReturnEmployeeId()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                EmployeeRepository repository = new EmployeeRepository(context);
                int result = await repository.CreateEmployeeAsync(_employees.First());

                Assert.NotEqual(0, result);
            }
        }

        [Fact]
        public async Task AddEmployee_WithInvalidEmail_ShouldThrowInputValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee invalidEmailEmployee = new Employee
                    { Name = "John Doe", Email = "invalidemail", IsArchived = false };

                EmployeeRepository repository = new EmployeeRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() =>
                    repository.CreateEmployeeAsync(invalidEmailEmployee));
            }
        }

        [Fact]
        public async Task AddEmployee_WithEmptyName_ShouldThrowInputValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee emptyNameEmployee = new Employee { Name = "", Email = "john@example.com", IsArchived = false };

                EmployeeRepository repository = new EmployeeRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() =>
                    repository.CreateEmployeeAsync(emptyNameEmployee));
            }
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithValidInput_ShouldReturnEmployeeId()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employeeRequest = new Employee
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    EmployeeId = 1
                };

                EmployeeRepository employeeService = new EmployeeRepository(context);

                int result = await employeeService.CreateEmployeeAsync(employeeRequest);

                Assert.Equal(employeeRequest.EmployeeId, result);
            }
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidInput_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employeeRequest = new Employee
                {
                    Email = "john.doe@example.com",
                    EmployeeId = 1
                };

                EmployeeRepository employeeRepository = new EmployeeRepository(context);

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    employeeRepository.CreateEmployeeAsync(employeeRequest));
            }
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidEmail_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employeeRequest1 = new Employee
                {
                    Email = "john.doe@example.com",
                    Name = "test"
                };

                Employee employeeRequest2 = new Employee
                {
                    Email = "john.doe@example.com",
                    Name = "t"
                };

                EmployeeRepository employeeRepository = new EmployeeRepository(context);
                await employeeRepository.CreateEmployeeAsync(employeeRequest1);

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    employeeRepository.CreateEmployeeAsync(employeeRequest2));
            }
        }

        [Fact]
        public async Task UpdateEmployeeIsArchived_ShouldUpdateIsArchived()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                Employee updatedEmployee = new Employee { EmployeeId = employee.EmployeeId, IsArchived = true };

                await repository.UpdateEmployeeIsArchivedAsync(MapEmployeeToDto(updatedEmployee));

                Employee resultEmployee = await context.Employees.FindAsync(employee.EmployeeId);
                Assert.NotNull(resultEmployee);
                Assert.True(resultEmployee.IsArchived);
            }
        }

        [Fact]
        public async Task UpdateEmployeeIsArchived_WithNonExistentEmployee_ShouldNotThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                Employee updatedEmployee = new Employee
                    { EmployeeId = 999, Name = "John Doe", Email = "john@example.com", IsArchived = true };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                NotFoundException actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateEmployeeIsArchivedAsync(MapEmployeeToDto(updatedEmployee)));
                Assert.NotNull(actualException);
            }
        }


        [Fact]
        public async Task UpdateEmployee_ShouldUpdateEmployee()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employee = new Employee { EmployeeId = 1, Name = "John Doe", Email = "john@example.com" };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                employee.Name = "Updated John";

                await repository.UpdateEmployeeAsync(MapEmployeeToDto(employee));
                Employee resultEmployee = await context.Employees.FindAsync(employee.EmployeeId);

                Assert.NotNull(resultEmployee);
                Assert.Equal("Updated John", resultEmployee.Name);
            }
        }

        [Fact]
        public async Task UpdateEmployee_WithNonExistentEmployee_ShouldNotThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                Employee updatedEmployee = new Employee
                    { EmployeeId = 999, Name = "Updated John", Email = "john@example.com", IsArchived = true };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                NotFoundException actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateEmployeeAsync(MapEmployeeToDto(updatedEmployee)));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task DeleteEmployee_WithExistingEmployee_ShouldDeleteEmployee()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddAsync(_employees.First());
                await context.SaveChangesAsync();

                EmployeeRepository repository = new EmployeeRepository(context);

                await repository.DeleteEmployeeAsync(_employees.First().EmployeeId);

                Employee deletedEmployee = await context.Employees.FindAsync(_employees.First().EmployeeId);
                Assert.Null(deletedEmployee);
            }
        }

        [Fact]
        public async Task DeleteEmployee_WithNonExistentEmployee_ShouldThrowNotFoundException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                EmployeeRepository repository = new EmployeeRepository(context);

                NotFoundException actualException = await Assert.ThrowsAsync<NotFoundException>(() =>
                    repository.DeleteEmployeeAsync(999));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task CheckEmailExistsAsync_EmailExists_ShouldThrowInputValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Employee existingEmployee = new Employee
                {
                    EmployeeId = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com"
                };
                
                Employee existingEmployee2 = new Employee
                {
                    EmployeeId = 2,
                    Name = "1",
                    Email = "1@" 
                };

                await context.Employees.AddAsync(existingEmployee);
                await context.Employees.AddAsync(existingEmployee2);
                await context.SaveChangesAsync();

                EmployeeRequestDto employeeRequest = new EmployeeRequestDto
                {
                    EmployeeId = 1,
                    Name = "1",
                    Email = "1@" 
                };

                EmployeeRepository employeeService = new EmployeeRepository(context);

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    employeeService.UpdateEmployeeAsync(employeeRequest));
            }
        }
        
    }
}