using BLL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class EmployeeControllerTests
{
    
    private IEnumerable<Employee> GetSampleEmployees()
    {
        return new List<Employee>
        {
            new Employee { EmployeeId = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new Employee { EmployeeId = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
            new Employee { EmployeeId = 3, Name = "Bob Smith", Email = "bob.smith@example.com", IsArchived = true},
        };
    }

    
    [Fact]
    public async Task GetPagedEmployees_ReturnsOkResultWithPagedEmployees()
    {
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(s => s.GetPagedEmployees(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleEmployees(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);
        IActionResult result = await controller.GetPagedEmployees("search", 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object employees = response.GetType().GetProperty("Employees")?.GetValue(response);

        Assert.NotNull(employees);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        employeeServiceMock.Verify(s => s.GetPagedEmployees(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task GetPagedArchivedEmployees_ShouldReturnOkResultWithPagedEmployees()
    {
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(s => s.GetPagedArchivedEmployees(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleEmployees(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        IActionResult result = await controller.GetPagedArchivedEmployees("search", 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object employees = response.GetType().GetProperty("Employees")?.GetValue(response);
        
        Assert.NotNull(employees);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        employeeServiceMock.Verify(s => s.GetPagedArchivedEmployees(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

    }

    [Fact]
    public async Task GetFilteredEmployeesAsync_ReturnsOkResultWithFilteredEmployees()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        IEnumerable<Employee> expectedEmployees = GetSampleEmployees(); 
        string searchField = "John"; // Set a search field value

        employeeServiceMock.Setup(s => s.GetFilteredEmployeesAsync(searchField))
            .ReturnsAsync(expectedEmployees);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.GetFilteredEmployeesAsync(searchField);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        IEnumerable<Employee> actualEmployees = okResult.Value as IEnumerable<Employee>;

        Assert.NotNull(actualEmployees);
        Assert.Equal(expectedEmployees, actualEmployees); // Assuming Employee class overrides Equals appropriately
        employeeServiceMock.Verify(s => s.GetFilteredEmployeesAsync(searchField), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ReturnsOkResultWithEmployee()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        Employee expectedEmployee = new Employee { EmployeeId = 1, Name = "John Doe", Email = "john.doe@example.com" }; // Create a sample employee
        int employeeId = 1; // Set the employee ID

        employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(employeeId))
            .ReturnsAsync(expectedEmployee);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.GetEmployeeByIdAsync(employeeId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Employee actualEmployee = okResult.Value as Employee;

        Assert.NotNull(actualEmployee);
        Assert.Equal(expectedEmployee, actualEmployee); // Assuming Employee class overrides Equals appropriately
        employeeServiceMock.Verify(s => s.GetEmployeeByIdAsync(employeeId), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ReturnsOkResultWithEmployeeId()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        int expectedEmployeeId = 1; // Set the expected employee ID
        EmployeeRequestDto employeeRequestDto = new EmployeeRequestDto { Name = "John Doe", Email = "john.doe@example.com" }; // Create a sample employee request DTO

        employeeServiceMock.Setup(s => s.CreateEmployeeAsync(employeeRequestDto))
            .ReturnsAsync(expectedEmployeeId);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.CreateEmployeeAsync(employeeRequestDto);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        int actualEmployeeId = (int)okResult.Value;

        Assert.Equal(expectedEmployeeId, actualEmployeeId);
        employeeServiceMock.Verify(s => s.CreateEmployeeAsync(employeeRequestDto), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchivedAsync_ReturnsOkResultWithMessage()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        EmployeeRequestDto employeeRequestDto = new EmployeeRequestDto { EmployeeId = 1, IsArchived = true }; // Set the sample data

        employeeServiceMock.Setup(s => s.UpdateEmployeeIsArchivedAsync(employeeRequestDto))
            .Returns(Task.CompletedTask);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.UpdateIsArchivedAsync(employeeRequestDto);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        dynamic response = okResult.Value as dynamic;

        employeeServiceMock.Verify(s => s.UpdateEmployeeIsArchivedAsync(employeeRequestDto), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ReturnsOkResultWithMessage()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        EmployeeRequestDto employeeRequestDto = new EmployeeRequestDto { EmployeeId = 1, /* other properties */ }; // Set the sample data

        employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(employeeRequestDto))
            .Returns(Task.CompletedTask);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.UpdateEmployeeAsync(employeeRequestDto);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        dynamic response = okResult.Value as dynamic;

        employeeServiceMock.Verify(s => s.UpdateEmployeeAsync(employeeRequestDto), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ReturnsOkResultWithMessage()
    {
        // Arrange
        Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
        int employeeIdToDelete = 1; // Set the employee ID to delete

        employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(employeeIdToDelete))
            .Returns(Task.CompletedTask);

        EmployeeController controller = new EmployeeController(employeeServiceMock.Object);

        // Act
        IActionResult result = await controller.DeleteEmployeeAsync(employeeIdToDelete);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        dynamic response = okResult.Value as dynamic;

        employeeServiceMock.Verify(s => s.DeleteEmployeeAsync(employeeIdToDelete), Times.Once);
    }



}