using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;
using PL.Models.Requests;

namespace Tests.Repositories
{

    public class LoginRepositoryTests
    {
        private readonly List<LoanHistory> _loanHistories = new()
        {
            new LoanHistory
            {
                LoanHistoryId = 1, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(0),
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product { ProductId = 1, Type = ProductType.Laptop, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 2, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(30),
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product { ProductId = 2, Type = ProductType.Laptop, SerialNumber = "123456" }
            },
            new LoanHistory
            {
                LoanHistoryId = 3, LoanDate = DateTime.Now, ReturnDate = null,
                Employee = new Employee { Name = "John Doe", Email = "john@example.com" },
                Product = new Product { ProductId = 3, Type = ProductType.Laptop, SerialNumber = "123456" }
            },
        };

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CheckCredentials_ValidCredentials_ShouldReturnUser()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new LoginRepository(context);

                var user = new User
                {
                    UserId = 1,
                    Email = "test@example.com",
                    PasswordHash = "zkiE/979pWcygZdGCnKiubLZywTAA0JJ6BeFfRSuw/Q=",
                    PasswordSalt = "Td2fD1/rI+0u0kt/RHSDMchDDJBX7EW6rmvUs7taLz4=" 
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var loginRequest = new LoginRequestDTO
                {
                    Email = "test@example.com",
                    Password = "1" 
                };

                var result = await userRepository.CheckCredentials(loginRequest);

                Assert.NotNull(result);
                Assert.Equal(user.UserId, result.UserId);
            }
        }

        [Fact]
        public async Task CheckCredentials_InvalidEmail_ShouldThrowInvalidCredentialsException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new LoginRepository(context);

                var loginRequest = new LoginRequestDTO
                {
                    Email = "nonexistent@example.com",
                    Password = "user_password" 
                };

                await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                    userRepository.CheckCredentials(loginRequest));
            }
        }

        [Fact]
        public async Task CheckCredentials_InvalidPassword_ShouldThrowInvalidCredentialsException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new LoginRepository(context);

                var user = new User
                {
                    UserId = 1,
                    Email = "test@example.com",
                    PasswordHash = "hashed_password", 
                    PasswordSalt = "salt" 
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var loginRequest = new LoginRequestDTO
                {
                    Email = "test@example.com",
                    Password = "incorrect_password"
                };

                await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                    userRepository.CheckCredentials(loginRequest));
            }
        }

    }
}