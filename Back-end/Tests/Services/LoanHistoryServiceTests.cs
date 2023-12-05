using BLL.Services;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using PL.Models;

namespace Tests.Services;

public class LoanHistoryServiceTests
{
    private static IEnumerable<LoanHistory> GetSampleLoanHistory()
    {
        return new List<LoanHistory>
        {
            new LoanHistory
            {
                LoanHistoryId = 1,
                Employee = new Employee { EmployeeId = 1, Name = "test", Email = "test@test.nl" },
                Product = new Product
                {
                    ProductId = 1, ExpirationDate = DateTime.Today.AddDays(15), SerialNumber = "x43124xr4td25",
                    PurchaseDate = DateTime.Today.AddDays(-10), Type = ProductType.Laptop
                },
                LoanDate = DateTime.Today.AddDays(-10),
                ReturnDate = DateTime.Today.AddDays(-1)
            },
            new LoanHistory
            {
                LoanHistoryId = 2,
                Employee = new Employee { EmployeeId = 2, Name = "test", Email = "test@test.nl" },
                Product = new Product
                {
                    ProductId = 2, ExpirationDate = DateTime.Today.AddDays(15), SerialNumber = "x43124xr4td25",
                    PurchaseDate = DateTime.Today.AddDays(-10), Type = ProductType.Laptop
                },
                LoanDate = DateTime.Today.AddDays(-10),
                ReturnDate = DateTime.Today.AddDays(-1)
            }
        };
    }


    [Fact]
    public async Task GetLoanHistoryByProductId_ShouldReturnPagedHistory()
    {
        const int productId = 1;
        const int page = 1;
        const int pageSize = 10;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock
            .Setup(repo => repo.GetLoanHistoryByProductId(productId, page, pageSize))
            .ReturnsAsync((GetSampleLoanHistory(), 20));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);

        (IEnumerable<object> pagedHistory, Pager pager) =
            await loanHistoryService.GetLoanHistoryByProductId(productId, page, pageSize);

        Assert.NotNull(pagedHistory);
        Assert.Equal(20, pager.TotalItems);
    }

    [Fact]
    public async Task GetLoanHistoryByProductId_ShouldHandleNoHistoryFound()
    {
        const int productId = 1;
        const int page = 1;
        const int pageSize = 10;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock
            .Setup(repo => repo.GetLoanHistoryByProductId(productId, page, pageSize))
            .ReturnsAsync((Enumerable.Empty<object>(), 0)); 

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);

        (IEnumerable<object> pagedHistory, Pager pager) =
            await loanHistoryService.GetLoanHistoryByProductId(productId, page, pageSize);

        Assert.Empty(pagedHistory);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public async Task GetLoanHistoryByCustomerId_ShouldReturnPagedHistory()
    {
        const int customerId = 1;
        const int page = 1;
        const int pageSize = 10;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock
            .Setup(repo => repo.GetLoanHistoryByCustomerId(customerId, page, pageSize))
            .ReturnsAsync((GetSampleLoanHistory(), 15));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);

        (IEnumerable<object> pagedHistory, Pager pager) =
            await loanHistoryService.GetLoanHistoryByCustomerId(customerId, page, pageSize);

        Assert.NotNull(pagedHistory);
        Assert.Equal(15, pager.TotalItems);
    }

    [Fact]
    public async Task GetLoanHistoryByCustomerId_ShouldHandleRepositoryException()
    {
        const int customerId = 1;
        const int page = 1;
        const int pageSize = 10;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock
            .Setup(repo => repo.GetLoanHistoryByCustomerId(customerId, page, pageSize))
            .ThrowsAsync(new Exception("repository exception"));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() =>
            loanHistoryService.GetLoanHistoryByCustomerId(customerId, page, pageSize));
    }

    [Fact]
    public async Task GetReturnDatesByProductId_ShouldReturnReturnDate()
    {
        const int productId = 1;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.GetReturnDatesByProductId(productId))
            .ReturnsAsync(DateTime.Today.AddDays(5));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        DateTime? returnDate = await loanHistoryService.GetReturnDatesByProductId(productId);

        Assert.NotNull(returnDate);
        Assert.Equal(DateTime.Today.AddDays(5), returnDate);
    }

    [Fact]
    public async Task GetReturnDatesByProductId_ShouldHandleRepositoryException()
    {
        const int productId = 1;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.GetReturnDatesByProductId(productId))
            .ThrowsAsync(new Exception("repository exception"));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => loanHistoryService.GetReturnDatesByProductId(productId));
    }

    [Fact]
    public async Task GetLatestLoanHistoryByProductId_ShouldReturnLatestLoanHistory()
    {
        const int productId = 1;
        LoanHistory expectedLoanHistory = new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Today.AddDays(-5), ReturnDate = DateTime.Today};

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.GetLatestLoanHistoryByProductId(productId))
            .ReturnsAsync(expectedLoanHistory);

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        LoanHistory result = await loanHistoryService.GetLatestLoanHistoryByProductId(productId);

        Assert.NotNull(result);
        Assert.Equal(expectedLoanHistory.LoanHistoryId, result.LoanHistoryId);
    }

    [Fact]
    public async Task GetLatestLoanHistoryByProductId_ShouldHandleRepositoryException()
    {
        const int productId = 1;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.GetLatestLoanHistoryByProductId(productId))
            .ThrowsAsync(new Exception("repository exception"));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => loanHistoryService.GetLatestLoanHistoryByProductId(productId));
    }

    [Fact]
    public async Task GetLatestLoanHistoryByProductId_ShouldHandleNullResult()
    {
        const int productId = 1;

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.GetLatestLoanHistoryByProductId(productId))
            .ReturnsAsync((LoanHistory)null);

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        LoanHistory result = await loanHistoryService.GetLatestLoanHistoryByProductId(productId);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateLoanHistory_ShouldCallRepositoryUpdateLoanHistory()
    {
        LoanHistory loanHistoryToUpdate = new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Today.AddDays(-5), ReturnDate = DateTime.Today };

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.UpdateLoanHistory(It.IsAny<LoanHistory>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await loanHistoryService.UpdateLoanHistory(loanHistoryToUpdate);
        loanHistoryRepositoryMock.Verify(repo => repo.UpdateLoanHistory(It.IsAny<LoanHistory>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLoanHistory_ShouldHandleRepositoryException()
    {
        LoanHistory loanHistoryToUpdate = new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Today.AddDays(-5), ReturnDate = DateTime.Today };

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.UpdateLoanHistory(It.IsAny<LoanHistory>()))
            .ThrowsAsync(new Exception("repository exception"));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => loanHistoryService.UpdateLoanHistory(loanHistoryToUpdate));
    }
    
    [Fact]
    public async Task PostLoanHistory_ShouldCallRepositoryPostLoanHistory()
    {
        LoanHistory loanHistoryToPost = new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Today.AddDays(-5), ReturnDate = DateTime.Today };

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.PostLoanHistory(It.IsAny<LoanHistory>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await loanHistoryService.PostLoanHistory(loanHistoryToPost);
        loanHistoryRepositoryMock.Verify(repo => repo.PostLoanHistory(It.IsAny<LoanHistory>()), Times.Once);
    }

    [Fact]
    public async Task PostLoanHistory_ShouldHandleRepositoryException()
    {
        LoanHistory loanHistoryToPost = new LoanHistory { LoanHistoryId = 1, LoanDate = DateTime.Today.AddDays(-5), ReturnDate = DateTime.Today };

        Mock<ILoanHistoryRepository> loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
        loanHistoryRepositoryMock.Setup(repo => repo.PostLoanHistory(It.IsAny<LoanHistory>()))
            .ThrowsAsync(new Exception("repository exception"));

        LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => loanHistoryService.PostLoanHistory(loanHistoryToPost));
    }
}