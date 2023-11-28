using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;

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

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllArchivedEmployees_ShouldReturnPagedResult()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                var nonExistentResult = await repository.GetAllArchivedEmployees("Doe", 1, 10);
                Assert.Empty(nonExistentResult.Item1);
                Assert.Equal(0, nonExistentResult.Item2);

                var result = await repository.GetAllArchivedEmployees("Regina", 1, 10);
                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);

                var pageTwoResult = await repository.GetAllArchivedEmployees("", 1, 5);
                Assert.Equal(2, pageTwoResult.Item2);
            }
        }

        [Fact]
        public async Task GetAllEmployee_ShouldReturnPagedResult()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                context.Employees.AddRange(_employees);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                var nonExistentResult = await repository.GetAllEmployee("Doe", 1, 10);
                Assert.Empty(nonExistentResult.Item1);
                Assert.Equal(0, nonExistentResult.Item2);

                var result = await repository.GetAllEmployee("John", 1, 10);
                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);

                var pageTwoResult = await repository.GetAllEmployee("jane", 2, 1);
                Assert.Equal(2, pageTwoResult.Item2);
            }
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldReturnFilteredResults()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);
                var result = await repository.GetFilteredEmployee("Regina");

                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal("Regina", result.First().Name);
            }
        }

        [Fact]
        public async Task GetFilteredEmployee_WithNoSearchField_ShouldReturnAllEmployees()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddRangeAsync(_employees);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);
                var result = await repository.GetFilteredEmployee("");

                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var employeeId = 1;
                var employee = new Employee
                    { EmployeeId = employeeId, Name = "John", Email = "john@example.com", IsArchived = false };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);
                var result = await repository.GetEmployeeById(employeeId);

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
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var nonExistentId = 999;

                var repository = new EmployeeRepository(context);
                var result = await repository.GetEmployeeById(nonExistentId);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddEmployee_WithValidEmployee_ShouldReturnEmployeeId()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new EmployeeRepository(context);
                var result = await repository.AddEmployee(_employees.First());

                Assert.NotEqual(0, result);
            }
        }

        [Fact]
        public async Task AddEmployee_WithInvalidEmail_ShouldThrowInputValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var invalidEmailEmployee = new Employee
                    { Name = "John Doe", Email = "invalidemail", IsArchived = false };

                var repository = new EmployeeRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddEmployee(invalidEmailEmployee));
            }
        }

        [Fact]
        public async Task AddEmployee_WithEmptyName_ShouldThrowInputValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var emptyNameEmployee = new Employee { Name = "", Email = "john@example.com", IsArchived = false };

                var repository = new EmployeeRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddEmployee(emptyNameEmployee));
            }
        }

        [Fact]
        public async Task AddEmployee_WithExistingEmail_ShouldNotAddAndReturnExistingEmployeeId()
        {
            var existingEmployee = new Employee { Name = "John Doe", Email = "john@example.com", IsArchived = false };

            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddAsync(existingEmployee);
                await context.SaveChangesAsync();
            }

            var newEmployeeWithExistingEmail = new Employee
                { Name = "Jane Doe", Email = "john@example.com", IsArchived = false };

            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new EmployeeRepository(context);
                var result = await repository.AddEmployee(newEmployeeWithExistingEmail);

                Assert.Equal(existingEmployee.EmployeeId, result);
            }
        }

        [Fact]
        public async Task UpdateEmployeeIsArchived_ShouldUpdateIsArchived()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                var updatedEmployee = new Employee { EmployeeId = employee.EmployeeId, IsArchived = true };

                await repository.UpdateEmployeeIsArchived(updatedEmployee);

                var resultEmployee = await context.Employees.FindAsync(employee.EmployeeId);
                Assert.NotNull(resultEmployee);
                Assert.True(resultEmployee.IsArchived);
            }
        }

        [Fact]
        public async Task UpdateEmployeeIsArchived_WithNonExistentEmployee_ShouldNotThrowException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                var updatedEmployee = new Employee
                    { EmployeeId = 999, Name = "John Doe", Email = "john@example.com", IsArchived = true };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateEmployeeIsArchived(updatedEmployee));
                Assert.NotNull(actualException);
            }
        }


        [Fact]
        public async Task UpdateEmployee_ShouldUpdateEmployee()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var employee = new Employee { EmployeeId = 1, Name = "John Doe", Email = "john@example.com" };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                employee.Name = "Updated John";

                await repository.UpdateEmployee(employee);
                var resultEmployee = await context.Employees.FindAsync(employee.EmployeeId);

                Assert.NotNull(resultEmployee);
                Assert.Equal("Updated John", resultEmployee.Name);
            }
        }

        [Fact]
        public async Task UpdateEmployee_WithNonExistentEmployee_ShouldNotThrowException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };

                var updatedEmployee = new Employee
                    { EmployeeId = 999, Name = "Updated John", Email = "john@example.com", IsArchived = true };

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateEmployee(updatedEmployee));
                Assert.NotNull(actualException);
            }
        }


        [Fact]
        public async Task DeleteEmployee_WithExistingEmployee_ShouldDeleteEmployee()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Employees.AddAsync(_employees.First());
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                await repository.DeleteEmployee(_employees.First().EmployeeId);

                var deletedEmployee = await context.Employees.FindAsync(_employees.First().EmployeeId);
                Assert.Null(deletedEmployee);
            }
        }

        [Fact]
        public async Task DeleteEmployee_WithNonExistentEmployee_ShouldThrowNotFoundException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new EmployeeRepository(context);

                var actualException = await Assert.ThrowsAsync<NotFoundException>(() =>
                    repository.DeleteEmployee(999));
                Assert.NotNull(actualException);
            }
        }
    }
}