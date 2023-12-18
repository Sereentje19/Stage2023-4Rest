using DAL.Data;
using DAL.Exceptions;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

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
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.SaveChangesAsync();

                const string code = "123456";

                User result = await userRepository.CreateResetCodeAsync(code, _users.First().Email);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);

                PasswordResetCode resetCode = await context.PasswordResetCode.FirstOrDefaultAsync(prc => prc.UserId == _users.First().UserId);
                Assert.NotNull(resetCode);
                Assert.Equal(code, resetCode.Code);
            }
        }

        [Fact]
        public async Task PostResetCode_UserNotFound_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);
                const string email = "nonexistent@example.com";
                const string code = "123456";

                await Assert.ThrowsAsync<InvalidCredentialsException>(() => userRepository.CreateResetCodeAsync(code, email));
            }
        }

        [Fact]
        public async Task PostResetCode_AddResetCode_Success()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.SaveChangesAsync();

                string code = "123456";

                User result = await userRepository.CreateResetCodeAsync(code, _users.First().Email);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);

                PasswordResetCode resetCode = await context.PasswordResetCode.FirstOrDefaultAsync(prc => prc.UserId == _users.First().UserId);
                Assert.NotNull(resetCode);
                Assert.Equal(code, resetCode.Code);
            }
        }

        [Fact]
        public async Task CheckEnteredCode_ValidCode_ShouldReturnUser()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.First());
                await context.SaveChangesAsync();

                User result = await userRepository.CheckEnteredCodeAsync(_users.First().Email, _passwordResetCode.First().Code);

                Assert.NotNull(result);
                Assert.Equal(_users.First().UserId, result.UserId);
            }
        }

        [Fact]
        public async Task CheckEnteredCode_InvalidCode_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.Last());
                await context.SaveChangesAsync();

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    userRepository.CheckEnteredCodeAsync(_users.First().Email, _passwordResetCode.Last().Code));
            }
        }

        [Fact]
        public async Task PostPassword_ValidCode_ShouldChangePassword()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.First());
                await context.SaveChangesAsync();

                await userRepository.CreatePasswordAsync(_users.First().Email, "new_password", _passwordResetCode.First().Code);

                User updatedUser = await context.Users.FindAsync(_users.First().UserId);
                Assert.NotNull(updatedUser);
            }
        }

        [Fact]
        public async Task PostPassword_InvalidCode_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userRepository = new PasswordResetRepository(context);

                await context.Users.AddAsync(_users.First());
                await context.PasswordResetCode.AddAsync(_passwordResetCode.Last());
                await context.SaveChangesAsync();

                await Assert.ThrowsAsync<InputValidationException>(() =>
                    userRepository.CreatePasswordAsync(_users.First().Email, "new_password", _passwordResetCode.Last().Code));
            }
        }
        
        
    }
}