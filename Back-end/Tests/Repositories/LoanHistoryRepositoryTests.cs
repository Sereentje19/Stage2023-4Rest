using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Models;
using PL.Models.Responses;

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
                Product = new Product { ProductId = 1, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 2, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(30),
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product { ProductId = 2, Type = new ProductType() { Id = 2, Name = "Laptop" }, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 3, LoanDate = DateTime.Now, ReturnDate = null,
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product { ProductId = 3, Type = new ProductType() { Id = 3, Name = "Laptop" }, SerialNumber = "123456" }
            },
        };

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
                (IEnumerable<object> result, int totalCount) = await repository.GetLoanHistoryByProductId(productId, page, pageSize);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(1, totalCount);

                IEnumerable<LoanHistoryResponse> loanHistories = (IEnumerable<LoanHistoryResponse>)result;
                LoanHistoryResponse loanHistoryResponse = loanHistories.First();

                Assert.Equal("Laptop", loanHistoryResponse.Type.Name);
                Assert.Equal("123456", loanHistoryResponse.SerialNumber);
                Assert.Equal("John Doe", loanHistoryResponse.Name);
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

                (IEnumerable<object> result, int totalCount) = await repository.GetLoanHistoryByProductId(productId, page, pageSize);

                Assert.NotNull(result);
                Assert.Empty(result);
                Assert.Equal(0, totalCount);
            }
        }


        [Fact]
        public async Task GetLoanHistoryByCustomerId_ShouldReturnLoanHistory()
        {
            const int customerId = 1;
            const int page = 1;
            const int pageSize = 10;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.LoanHistory.AddAsync(_loanHistories.First());
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                (IEnumerable<object> result, int totalCount) = await repository.GetLoanHistoryByCustomerId(customerId, page, pageSize);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(1, totalCount);

                IEnumerable<LoanHistoryResponse> loanHistories = (IEnumerable<LoanHistoryResponse>)result;
                LoanHistoryResponse loanHistoryResponse = loanHistories.First();

                Assert.Equal("Laptop", loanHistoryResponse.Type.Name);
                Assert.Equal("123456", loanHistoryResponse.SerialNumber);
                Assert.Equal("John Doe", loanHistoryResponse.Name);
                Assert.Equal("john@example.com", loanHistoryResponse.Email);
                Assert.Equal(_loanHistories.First().LoanDate, loanHistoryResponse.LoanDate);
                Assert.Equal(_loanHistories.First().ReturnDate, loanHistoryResponse.ReturnDate);
            }
        }

        [Fact]
        public async Task GetLoanHistoryByCustomerId_ShouldReturnEmptyResult()
        {
            const int customerId = 999;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                (IEnumerable<object> result, int totalCount) = await repository.GetLoanHistoryByCustomerId(customerId, 1, 10);

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

                DateTime? latestReturnDate = await repository.GetReturnDatesByProductId(productId);

                Assert.NotNull(latestReturnDate);
                Assert.Equal(_loanHistories[0].ReturnDate, latestReturnDate);
            }
        }

        [Fact]
        public async Task GetReturnDatesByProductId_ShouldReturnCurrentDateTimeWhenNoLoanHistory()
        {
            int productId = 999;

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                DateTime? result = await repository.GetReturnDatesByProductId(productId);

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
                LoanHistory latestLoanHistory = await repository.GetLatestLoanHistoryByProductId(productId);

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
                LoanHistory result = await repository.GetLatestLoanHistoryByProductId(productId);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateLoanHistory_ShouldUpdateLoanHistory()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Product product = new Product { ProductId = 4, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" };
                Employee employee = new Employee { EmployeeId = 4, Name = "John Doe", Email = "john@example.com", IsArchived = false };
                LoanHistory loanHistory = new LoanHistory { LoanHistoryId = 4, Product = product, Employee = employee, LoanDate = DateTime.Now.AddDays(-7), ReturnDate = null};

                await context.Products.AddAsync(product);
                await context.Employees.AddAsync(employee);
                await context.LoanHistory.AddAsync(loanHistory);
                await context.SaveChangesAsync();
                
                LoanHistoryRepository repository = new LoanHistoryRepository(context);
                await repository.UpdateLoanHistory(loanHistory);
                
                LoanHistory updatedLoanHistory = await context.LoanHistory.FindAsync(loanHistory.LoanHistoryId);
                Assert.NotNull(updatedLoanHistory);
                Assert.Equal(DateTime.Now.Date, updatedLoanHistory.ReturnDate?.Date);
            }
        }

        [Fact]
        public async Task PostLoanHistory_ShouldAddLoanHistoryToDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Product product = new Product { ProductId = 1, Type = new ProductType() { Id = 1, Name = "Laptop" }, SerialNumber = "123456" };
                Employee employee = new Employee { EmployeeId = 1, Name = "John Doe", Email = "john@example.com", IsArchived = false };
                LoanHistory loanHistoryToPost = new LoanHistory { LoanHistoryId = 1, Product = product, Employee = employee, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(7) };

                await context.Products.AddAsync(product);
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                LoanHistoryRepository repository = new LoanHistoryRepository(context);

                await repository.PostLoanHistory(loanHistoryToPost);
                LoanHistory loanHistoryInDatabase = await context.LoanHistory.FindAsync(loanHistoryToPost.LoanHistoryId);

                Assert.NotNull(loanHistoryInDatabase);
                Assert.Equal(loanHistoryToPost.LoanDate, loanHistoryInDatabase.LoanDate);
                Assert.Equal(loanHistoryToPost.ReturnDate, loanHistoryInDatabase.ReturnDate);
                Assert.Equal(loanHistoryToPost.Product.ProductId, loanHistoryInDatabase.Product.ProductId);
                Assert.Equal(loanHistoryToPost.Employee.EmployeeId, loanHistoryInDatabase.Employee.EmployeeId);
            }
        }

        
    }
}