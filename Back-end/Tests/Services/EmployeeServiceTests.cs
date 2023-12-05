using BLL.Services;
using DAL.Repositories;
using Moq;
using PL.Models;

namespace Tests.Services
{

    public class EmployeeServiceTests
    {
        private static IEnumerable<Employee> GetSampleEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    Name = "John Doe",
                    Email = "john@example.com"
                },
                new Employee
                {
                    EmployeeId = 2,
                    Name = "Jane Doe",
                    Email = "jane@example.com"
                },
                new Employee
                {
                    EmployeeId = 1,
                    Name = "Archived John Doe",
                    Email = "archivedjohn@example.com",
                    IsArchived = true
                    // Add other properties as needed
                },
                new Employee
                {
                    EmployeeId = 2,
                    Name = "Archived Jane Doe",
                    Email = "archivedjane@example.com",
                    IsArchived = true
                    // Add other properties as needed
                },
            };
        }

        [Fact]
        public async Task GetPagedEmployee_ShouldReturnPagedEmployees()
        {
            const string searchfield = "example";
            const int page = 1;
            const int pageSize = 5;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllEmployee(searchfield, page, pageSize))
                .ReturnsAsync((GetSampleEmployees(), 2));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            (IEnumerable<object> pagedEmployees, Pager pager) =
                await employeeService.GetPagedEmployee(searchfield, page, pageSize);

            Assert.NotNull(pagedEmployees);
            Assert.Equal(2, pager.TotalItems);
        }

        [Fact]
        public async Task GetPagedEmployee_ShouldHandleRepositoryException()
        {
            const string searchfield = "example";
            const int page = 1;
            const int pageSize = 5;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllEmployee(searchfield, page, pageSize))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.GetPagedEmployee(searchfield, page, pageSize));
        }

        [Fact]
        public async Task GetPagedArchivedEmployee_ShouldReturnPagedArchivedEmployees()
        {
            const string searchfield = "example";
            const int page = 1;
            const int pageSize = 5;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllArchivedEmployees(searchfield, page, pageSize))
                .ReturnsAsync((GetSampleEmployees(), 2));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            (IEnumerable<object> pagedArchivedEmployees, Pager pager) =
                await employeeService.GetPagedArchivedEmployee(searchfield, page, pageSize);

            Assert.NotNull(pagedArchivedEmployees);
            Assert.Equal(2, pager.TotalItems);
        }

        [Fact]
        public async Task GetPagedArchivedEmployee_ShouldHandleRepositoryException()
        {
            const string searchfield = "example";
            const int page = 1;
            const int pageSize = 5;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllArchivedEmployees(searchfield, page, pageSize))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() =>
                employeeService.GetPagedArchivedEmployee(searchfield, page, pageSize));
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldReturnFilteredEmployees()
        {
            const string searchfield = "example";

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetFilteredEmployee(searchfield))
                .ReturnsAsync(GetSampleEmployees()); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            IEnumerable<Employee> filteredEmployees = await employeeService.GetFilteredEmployee(searchfield);
            Assert.NotNull(filteredEmployees);
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldHandleRepositoryException()
        {
            const string searchfield = "example";

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetFilteredEmployee(searchfield))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(() => employeeService.GetFilteredEmployee(searchfield));
        }
        
        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee()
        {
            const int employeeId = 1; 

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetEmployeeById(employeeId))
                .ReturnsAsync(GetSampleEmployees().First()); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            Employee resultEmployee = await employeeService.GetEmployeeById(employeeId);

            Assert.NotNull(resultEmployee);
            Assert.Equal(employeeId, resultEmployee.EmployeeId);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldHandleRepositoryException()
        {
            const int employeeId = 1; 

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetEmployeeById(employeeId))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.GetEmployeeById(employeeId));
        }

        [Fact]
        public async Task PostEmployee_ShouldReturnEmployeeId()
        {
            Employee employeeToPost = new Employee
            {
                Name = "John Doe",
                Email = "john@example.com"
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.AddEmployee(It.IsAny<Employee>()))
                .ReturnsAsync(1); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            int resultEmployeeId = await employeeService.PostEmployee(employeeToPost);

            Assert.Equal(1, resultEmployeeId); 
        }

        [Fact]
        public async Task PostEmployee_ShouldHandleRepositoryException()
        {
            Employee employeeToPost = new Employee
            {
                Name = "John Doe",
                Email = "john@example.com"
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.AddEmployee(It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.PostEmployee(employeeToPost));
        }

        [Fact]
        public async Task PutEmployeeIsArchived_ShouldCallRepositoryUpdateEmployeeIsArchived()
        {
            Employee employeeToUpdate = new Employee
            {
                EmployeeId = 1,
                Name = "John Doe",
                Email = "john@example.com",
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeIsArchived(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.PutEmployeeIsArchived(employeeToUpdate);

            employeeRepositoryMock.Verify(repo => repo.UpdateEmployeeIsArchived(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task PutEmployeeIsArchived_ShouldHandleRepositoryException()
        {
            Employee employeeToUpdate = new Employee
            {
                EmployeeId = 1,
                Name = "John Doe",
                Email = "john@example.com",
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeIsArchived(It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.PutEmployeeIsArchived(employeeToUpdate));
        }

        [Fact]
        public async Task PutEmployee_ShouldCallRepositoryUpdateEmployee()
        {
            Employee employeeToUpdate = new Employee
            {
                EmployeeId = 1,
                Name = "John Doe",
                Email = "john@example.com",
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployee(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.PutEmployee(employeeToUpdate);

            employeeRepositoryMock.Verify(repo => repo.UpdateEmployee(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task PutEmployee_ShouldHandleRepositoryException()
        {
            Employee employeeToUpdate = new Employee
            {
                EmployeeId = 1,
                Name = "John Doe",
                Email = "john@example.com",
            };

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployee(It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.PutEmployee(employeeToUpdate));
        }

        [Fact]
        public async Task DeleteEmployee_ShouldCallRepositoryDeleteEmployee()
        {
            const int employeeIdToDelete = 1;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.DeleteEmployee(It.IsAny<int>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.DeleteEmployee(employeeIdToDelete);

            employeeRepositoryMock.Verify(repo => repo.DeleteEmployee(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldHandleRepositoryException()
        {
            const int employeeIdToDelete = 1;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.DeleteEmployee(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.DeleteEmployee(employeeIdToDelete));
        }

        



        






    }
}