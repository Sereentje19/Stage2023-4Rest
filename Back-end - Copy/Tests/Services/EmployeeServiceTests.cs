using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Repositories;
using Moq;

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
                },
                new Employee
                {
                    EmployeeId = 2,
                    Name = "Archived Jane Doe",
                    Email = "archivedjane@example.com",
                    IsArchived = true
                },
            };
        }

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

        [Fact]
        public async Task GetPagedEmployee_ShouldReturnPagedEmployees()
        {
            const string searchfield = "example";
            const int page = 1;
            const int pageSize = 5;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllEmployees(searchfield, page, pageSize))
                .ReturnsAsync((GetSampleEmployees(), 2));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            (IEnumerable<object> pagedEmployees, Pager pager) =
                await employeeService.GetPagedEmployees(searchfield, page, pageSize);

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
            employeeRepositoryMock.Setup(repo => repo.GetAllEmployees(searchfield, page, pageSize))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.GetPagedEmployees(searchfield, page, pageSize));
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
                await employeeService.GetPagedArchivedEmployees(searchfield, page, pageSize);

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
                employeeService.GetPagedArchivedEmployees(searchfield, page, pageSize));
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldReturnFilteredEmployees()
        {
            const string searchfield = "example";

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetFilteredEmployeesAsync(searchfield))
                .ReturnsAsync(GetSampleEmployees()); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            IEnumerable<Employee> filteredEmployees = await employeeService.GetFilteredEmployeesAsync(searchfield);
            Assert.NotNull(filteredEmployees);
        }

        [Fact]
        public async Task GetFilteredEmployee_ShouldHandleRepositoryException()
        {
            const string searchfield = "example";

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetFilteredEmployeesAsync(searchfield))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(() => employeeService.GetFilteredEmployeesAsync(searchfield));
        }
        
        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee()
        {
            const int employeeId = 1; 

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(GetSampleEmployees().First()); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);

            Employee resultEmployee = await employeeService.GetEmployeeByIdAsync(employeeId);

            Assert.NotNull(resultEmployee);
            Assert.Equal(employeeId, resultEmployee.EmployeeId);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldHandleRepositoryException()
        {
            const int employeeId = 1; 

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetEmployeeByIdAsync(employeeId))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.GetEmployeeByIdAsync(employeeId));
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
            employeeRepositoryMock.Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRequestDto>()))
                .ReturnsAsync(1); 

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            int resultEmployeeId = await employeeService.CreateEmployeeAsync(MapEmployeeToDto(employeeToPost));

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
            employeeRepositoryMock.Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRequestDto>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.CreateEmployeeAsync(MapEmployeeToDto(employeeToPost)));
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
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeIsArchivedAsync(It.IsAny<EmployeeRequestDto>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.UpdateEmployeeIsArchivedAsync(MapEmployeeToDto(employeeToUpdate));

            employeeRepositoryMock.Verify(repo => repo.UpdateEmployeeIsArchivedAsync(It.IsAny<EmployeeRequestDto>()), Times.Once);
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
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeIsArchivedAsync(It.IsAny<EmployeeRequestDto>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.UpdateEmployeeIsArchivedAsync(MapEmployeeToDto(employeeToUpdate)));
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
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRequestDto>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.UpdateEmployeeAsync(MapEmployeeToDto(employeeToUpdate));

            employeeRepositoryMock.Verify(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRequestDto>()), Times.Once);
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
            employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRequestDto>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.UpdateEmployeeAsync(MapEmployeeToDto(employeeToUpdate)));
        }

        [Fact]
        public async Task DeleteEmployee_ShouldCallRepositoryDeleteEmployee()
        {
            const int employeeIdToDelete = 1;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.DeleteEmployeeAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await employeeService.DeleteEmployeeAsync(employeeIdToDelete);

            employeeRepositoryMock.Verify(repo => repo.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldHandleRepositoryException()
        {
            const int employeeIdToDelete = 1;

            Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.DeleteEmployeeAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            EmployeeService employeeService = new EmployeeService(employeeRepositoryMock.Object);
            await Assert.ThrowsAsync<Exception>(() => employeeService.DeleteEmployeeAsync(employeeIdToDelete));
        }

        



        






    }
}