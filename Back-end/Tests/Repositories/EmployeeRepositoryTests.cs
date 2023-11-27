using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
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
    }
}