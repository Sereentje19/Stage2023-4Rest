﻿using BLL.Services;
using DAL.Data;
using DAL.Exceptions;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories
{
    public class LoanHistoryRepositoryTests
    {
        private readonly List<LoanHistory> _loanHistories = new()
        {
            new LoanHistory
            {
                LoanHistoryId = 1, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(0),
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product
                    { ProductId = 1, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 2, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(30),
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product
                    { ProductId = 2, Type = new ProductType() { Id = 2, Name = "Laptop" }, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 3, LoanDate = DateTime.Now, ReturnDate = null,
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product
                    { ProductId = 3, Type = new ProductType() { Id = 3, Name = "Laptop" }, SerialNumber = "123456" }
            },
        };

        private static LoanHistoryRequestDto MapLoanhistoryToDto(LoanHistory lh)
        {
            return new LoanHistoryRequestDto()
            {
                LoanHistoryId = lh.LoanHistoryId,
                LoanDate = lh.LoanDate,
                ReturnDate = lh.ReturnDate,
                Employee = lh.Employee,
                Product = lh.Product,
            };
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetLoanHistoryByProductId_ShouldReturnLoanHistory()
        {
            const int productId = 1;
            const int page = 1;
            const int pageSize = 10;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.LoanHistory.AddAsync(_loanHistories.First());
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                (IEnumerable<object> result, int totalCount) =
                    await repository.GetLoanHistoryByProductIdAsync(productId, page, pageSize);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(1, totalCount);

                IEnumerable<LoanHistoryResponseDto> loanHistories = (IEnumerable<LoanHistoryResponseDto>)result;
                LoanHistoryResponseDto loanHistoryResponseDto = loanHistories.First();

                Assert.Equal("Laptop", loanHistoryResponseDto.Type.Name);
                Assert.Equal("123456", loanHistoryResponseDto.SerialNumber);
                Assert.Equal("John Doe", loanHistoryResponseDto.Name);
            }
        }

        [Fact]
        public async Task GetLoanHistoryByProductId_ShouldReturnEmptyListWhenNoData()
        {
            const int productId = 1;
            const int page = 1;
            const int pageSize = 10;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                (IEnumerable<object> result, int totalCount) =
                    await repository.GetLoanHistoryByProductIdAsync(productId, page, pageSize);

                Assert.NotNull(result);
                Assert.Empty(result);
                Assert.Equal(0, totalCount);
            }
        }


        [Fact]
        public async Task GetLoanHistoryByEmployeeId_ShouldReturnLoanHistory()
        {
            const int customerId = 1;
            const int page = 1;
            const int pageSize = 10;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.LoanHistory.AddAsync(_loanHistories.First());
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                (IEnumerable<object> result, int totalCount) =
                    await repository.GetLoanHistoryByEmployeeIdAsync(customerId, page, pageSize);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(1, totalCount);

                IEnumerable<LoanHistoryResponseDto> loanHistories = (IEnumerable<LoanHistoryResponseDto>)result;
                LoanHistoryResponseDto loanHistoryResponseDto = loanHistories.First();

                Assert.Equal("Laptop", loanHistoryResponseDto.Type.Name);
                Assert.Equal("123456", loanHistoryResponseDto.SerialNumber);
                Assert.Equal("John Doe", loanHistoryResponseDto.Name);
                Assert.Equal("john@example.com", loanHistoryResponseDto.Email);
                Assert.Equal(_loanHistories.First().LoanDate, loanHistoryResponseDto.LoanDate);
                Assert.Equal(_loanHistories.First().ReturnDate, loanHistoryResponseDto.ReturnDate);
            }
        }

        [Fact]
        public async Task GetLoanHistoryByEmployeeId_ShouldReturnEmptyResult()
        {
            const int customerId = 999;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                (IEnumerable<object> result, int totalCount) =
                    await repository.GetLoanHistoryByEmployeeIdAsync(customerId, 1, 10);

                Assert.NotNull(result);
                Assert.Empty(result);
                Assert.Equal(0, totalCount);
            }
        }

        [Fact]
        public async Task GetReturnDatesByProductId_ShouldReturnLatestReturnDate()
        {
            const int productId = 1;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.LoanHistory.AddRangeAsync(_loanHistories);
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                DateTime? latestReturnDate = await repository.GetReturnDatesByProductIdAsync(productId);

                Assert.NotNull(latestReturnDate);
                Assert.Equal(_loanHistories[0].ReturnDate, latestReturnDate);
            }
        }

        [Fact]
        public async Task GetReturnDatesByProductId_ShouldReturnCurrentDateTimeWhenNoLoanHistory()
        {
            const int productId = 999;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                DateTime? result = await repository.GetReturnDatesByProductIdAsync(productId);

                Assert.NotNull(result);
                Assert.Equal(DateTime.Now.Date, result.Value.Date);
            }
        }

        [Fact]
        public async Task GetLatestLoanHistoryByProductId_ShouldReturnLatestLoanHistory()
        {
            const int productId = 1;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.LoanHistory.AddRangeAsync(_loanHistories);
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                LoanHistory latestLoanHistory = await repository.GetLatestLoanHistoryByProductIdAsync(productId);

                Assert.NotNull(latestLoanHistory);
                Assert.Equal(_loanHistories[0].ReturnDate, latestLoanHistory.ReturnDate);
                Assert.Equal(_loanHistories[0].Product.ProductId, latestLoanHistory.Product.ProductId);
            }
        }

        [Fact]
        public async Task GetLatestLoanHistoryByProductId_ShouldReturnNullWhenNoLoanHistory()
        {
            const int productId = 999;

            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                LoanHistory result = await repository.GetLatestLoanHistoryByProductIdAsync(productId);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateLoanHistoryAsync_WithValidInput_ShouldUpdateLoanHistory()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository loanHistoryRepository = new LoanHistoryRepository(context);
                LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepository);

                LoanHistory updatedLoanHistoryEntity = new LoanHistory
                {
                    LoanHistoryId = 5,
                    LoanDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(0),
                    Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                    Product = new Product
                        { ProductId = 5, Type = new ProductType() { Id = 5, Name = "Laptop" }, SerialNumber = "123456" }
                };

                await context.AddAsync(updatedLoanHistoryEntity);
                await context.SaveChangesAsync();

                await loanHistoryService.UpdateLoanHistoryAsync(new LoanHistoryRequestDto
                {
                    LoanHistoryId = 5,
                    LoanDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(0),
                    Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                    Product = new Product
                        { ProductId = 5, Type = new ProductType() { Id = 5, Name = "Laptop" }, SerialNumber = "123456" }
                });

                LoanHistory updatedLoanHistory = await context.LoanHistory.FindAsync(5); 
                Assert.NotNull(updatedLoanHistory);
                Assert.Equal(updatedLoanHistoryEntity.Product.ProductId, updatedLoanHistory.Product.ProductId);
                Assert.Equal(updatedLoanHistoryEntity.Employee.EmployeeId, updatedLoanHistory.Employee.EmployeeId);
                Assert.NotNull(updatedLoanHistory.ReturnDate);
                Assert.Equal(updatedLoanHistoryEntity.LoanDate, updatedLoanHistory.LoanDate);
            }
        }
        
        [Fact]
        public async Task UpdateLoanHistoryAsync_WithInvalidLoanHistoryId_ShouldThrowNotFoundException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository loanHistoryRepository = new LoanHistoryRepository(context);
                LoanHistoryService loanHistoryService = new LoanHistoryService(loanHistoryRepository);

                const int invalidLoanHistoryId = -1;

                LoanHistoryRequestDto invalidLoanHistoryDto = new LoanHistoryRequestDto
                {
                    LoanHistoryId = invalidLoanHistoryId,
                    LoanDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(0),
                    Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                    Product = new Product { ProductId = 1, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" }
                };

                NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                    loanHistoryService.UpdateLoanHistoryAsync(invalidLoanHistoryDto));

                Assert.Equal("LoanHistory not found", exception.Message);
            }
        }


        [Fact]
        public async Task PostLoanHistory_ShouldAddLoanHistoryToDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Product product = new Product
                    { ProductId = 1, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" };
                Employee employee = new Employee
                    { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };
                LoanHistory loanHistoryToPost = new LoanHistory
                {
                    LoanHistoryId = 1, Product = product, Employee = employee, LoanDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7)
                };

                await context.Products.AddAsync(product);
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                await repository.CreateLoanHistoryAsync(MapLoanhistoryToDto(loanHistoryToPost));
                LoanHistory loanHistoryInDatabase =
                    await context.LoanHistory.FindAsync(loanHistoryToPost.LoanHistoryId);

                Assert.NotNull(loanHistoryInDatabase);
                Assert.Equal(loanHistoryToPost.LoanDate, loanHistoryInDatabase.LoanDate);
                Assert.Equal(loanHistoryToPost.ReturnDate, loanHistoryInDatabase.ReturnDate);
                Assert.Equal(loanHistoryToPost.Product.ProductId, loanHistoryInDatabase.Product.ProductId);
                Assert.Equal(loanHistoryToPost.Employee.EmployeeId, loanHistoryInDatabase.Employee.EmployeeId);
            }
        }
    }
}