using BLL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class LoanHistoryControllerTests
{
    
    
    [Fact]
    public async Task GetReturnDatesByProductIdAsync_ReturnsOkResultWithReturnDates()
    {
        const int productId = 1;
        DateTime? sampleReturnDate = DateTime.Now.AddDays(1);

        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        loanHistoryServiceMock.Setup(s => s.GetReturnDatesByProductIdAsync(productId))
            .ReturnsAsync(sampleReturnDate);

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);
        IActionResult result = await controller.GetReturnDatesByProductIdAsync(productId);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True(okResult.Value is DateTime? || okResult.Value is DateTime);

        DateTime? actualReturnDate = okResult.Value is DateTime dateTimeValue
            ? (DateTime?)dateTimeValue
            : okResult.Value as DateTime?;

        Assert.NotNull(actualReturnDate);
        Assert.Equal(sampleReturnDate, actualReturnDate);

        loanHistoryServiceMock.Verify(s => s.GetReturnDatesByProductIdAsync(productId), Times.Once);
    }





    
    [Fact]
    public async Task GetLatestLoanHistoryByProductIdAsync_ReturnsOkResultWithLoanHistory()
    {
        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        const int productId = 1;

        LoanHistory expectedLoanHistory = new LoanHistory { LoanHistoryId = 42, Product = new Product { ProductId = productId } };
        loanHistoryServiceMock.Setup(s => s.GetLatestLoanHistoryByProductIdAsync(productId))
            .ReturnsAsync(expectedLoanHistory);

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);

        IActionResult result = await controller.GetLatestLoanHistoryByProductIdAsync(productId);
 
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        LoanHistory actualLoanHistory = okResult.Value as LoanHistory;

        Assert.NotNull(actualLoanHistory);
        Assert.Equal(expectedLoanHistory, actualLoanHistory);
        loanHistoryServiceMock.Verify(s => s.GetLatestLoanHistoryByProductIdAsync(productId), Times.Once);
    }
    
    [Fact]
    public async Task CreateLoanHistoryAsync_ReturnsOkResult()
    {
        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        LoanHistoryRequestDto loanHistoryRequestDto = new LoanHistoryRequestDto();

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);
        IActionResult result = await controller.CreateLoanHistoryAsync(loanHistoryRequestDto);
        Assert.IsType<OkObjectResult>(result);

        loanHistoryServiceMock.Verify(s => s.CreateLoanHistoryAsync(It.IsAny<LoanHistoryRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLoanHistoryAsync_ReturnsOkResult()
    {
        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        LoanHistoryRequestDto loanHistoryRequestDto = new LoanHistoryRequestDto();

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);
        IActionResult result = await controller.UpdateLoanHistoryAsync(loanHistoryRequestDto);
        Assert.IsType<OkObjectResult>(result);

        loanHistoryServiceMock.Verify(s => s.UpdateLoanHistoryAsync(It.IsAny<LoanHistoryRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task GetLoanHistoryByEmployeeIdAsync_ReturnsOkResultWithPagedLoanHistory()
    {
        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        loanHistoryServiceMock.Setup(s => s.GetLoanHistoryByEmployeeIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleLoanHistory(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);
        IActionResult result = await controller.GetLoanHistoryByEmployeeIdAsync(1, 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object loanHistory = response.GetType().GetProperty("LoanHistory")?.GetValue(response);

        Assert.NotNull(loanHistory);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        loanHistoryServiceMock.Verify(s => s.GetLoanHistoryByEmployeeIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task GetLoanHistoryByProductIdAsync_ReturnsOkResultWithPagedLoanHistory()
    {
        Mock<ILoanHistoryService> loanHistoryServiceMock = new Mock<ILoanHistoryService>();
        loanHistoryServiceMock.Setup(s => s.GetLoanHistoryByProductIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleLoanHistory(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        LoanHistoryController controller = new LoanHistoryController(loanHistoryServiceMock.Object);
        IActionResult result = await controller.GetLoanHistoryByProductIdAsync(1, 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object loanHistory = response.GetType().GetProperty("LoanHistory")?.GetValue(response);

        Assert.NotNull(loanHistory);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        loanHistoryServiceMock.Verify(s => s.GetLoanHistoryByProductIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }


    private IEnumerable<LoanHistory> GetSampleLoanHistory()
    {
        return new List<LoanHistory>
        {
            new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Now, Employee = new Employee{ EmployeeId = 1, Name = "name", Email = "@email" } },
            new LoanHistory { LoanHistoryId = 2, LoanDate = DateTime.Now, Employee = new Employee{ Email = "@email" }, Product = new Product{ ProductId = 1, SerialNumber = "derio"} },
            new LoanHistory { LoanHistoryId = 3, LoanDate = DateTime.Now, Employee = new Employee{ Name = "name", Email = "@email" }},
        };
    }

    

}