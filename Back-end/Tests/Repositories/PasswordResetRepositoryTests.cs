using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;

namespace Tests.Repositories
{
    public class PasswordResetRepositoryTests
    {
        private readonly List<User> _users = new()
        {
            new User
            {
                UserId = 1, Email = "test@example.com", Name = "test"
            },
            new User
            {
                UserId = 1, Email = "test@example.com", Name = "test"
            },
        };

        private readonly List<PasswordResetCode> _passwordResetCode = new()
        {
            new PasswordResetCode
            {
                UserId = 1, Code = "123456", ExpirationTime = DateTime.UtcNow.AddMinutes(5)
            },
            new PasswordResetCode
            {
                UserId = 1, Code = "123456", ExpirationTime = DateTime.UtcNow.AddMinutes(-1)
            }
        };
        
        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task PostResetCode_UserFound_ShouldAddResetCode()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.SaveChangesAsync();

                const string code = "123456";

                var result = await userRepository.PostResetCode(code, _users.First().Email);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);

                var resetCode = await context.PasswordResetCode.FirstOrDefaultAsync(prc => prc.UserId == _users.First().UserId);
                Assert.NotNull(resetCode);
                Assert.Equal(code, resetCode.Code);
            }
        }

        [Fact]
        public async Task PostResetCode_UserNotFound_ShouldThrowException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);
                const string email = "nonexistent@example.com";
                const string code = "123456";

                await Assert.ThrowsAsync<InvalidCredentialsException>(() => userRepository.PostResetCode(code, email));
            }
        }

        [Fact]
        public async Task PostResetCode_AddResetCode_Success()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.SaveChangesAsync();

                var code = "123456";

                var result = await userRepository.PostResetCode(code, _users.First().Email);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);

                var resetCode = await context.PasswordResetCode.FirstOrDefaultAsync(prc => prc.UserId == _users.First().UserId);
                Assert.NotNull(resetCode);
                Assert.Equal(code, resetCode.Code);
            }
        }

        [Fact]
        public async Task CheckEnteredCode_ValidCode_ShouldReturnUser()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.First());
                await context.SaveChangesAsync();

                var result = await userRepository.CheckEnteredCode(_users.First().Email, _passwordResetCode.First().Code);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);
            }
        }

        [Fact]
        public async Task CheckEnteredCode_InvalidCode_ShouldThrowException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.Last());
                await context.SaveChangesAsync();

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    userRepository.CheckEnteredCode(_users.First().Email, _passwordResetCode.Last().Code));
            }
        }

        [Fact]
        public async Task PostPassword_ValidCode_ShouldChangePassword()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.First());
                await context.SaveChangesAsync();

                await userRepository.PostPassword(_users.First().Email, "new_password", _passwordResetCode.First().Code);

                var updatedUser = await context.Users.FindAsync(_users.First().UserId);
                Assert.NotNull(updatedUser);
            }
        }

        [Fact]
        public async Task PostPassword_InvalidCode_ShouldThrowException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.Last());
                await context.SaveChangesAsync();

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    userRepository.PostPassword(_users.First().Email, "new_password", _passwordResetCode.Last().Code));
            }
        }
        
        
    }
}