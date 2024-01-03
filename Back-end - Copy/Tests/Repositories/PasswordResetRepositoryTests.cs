using BLL.Services;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

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

                await userRepository.CreatePasswordAsync(_users.First().Email, "new_password1@2freD", _passwordResetCode.First().Code);

                User updatedUser = await context.Users.FindAsync(_users.First().UserId);
                Assert.NotNull(updatedUser);
            }
        }
        
        [Fact]
        public async Task CreatePasswordAsync_InvalidPassword_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                User testUser = new User { UserId = 1, Name = "Test", Email = "test@example.com" };
                PasswordResetCode prc = new PasswordResetCode()
                {
                    Code = "123456",
                    ExpirationTime = DateTime.Now.AddMinutes(-5),
                    UserId = 1,
                    ResetCodeId = 1
                };

                await context.AddAsync(prc);
                await context.AddAsync(testUser);
                await context.SaveChangesAsync();
                
                PasswordResetRepository passwordService = new PasswordResetRepository(context);

                InputValidationException exception = await Assert.ThrowsAsync<InputValidationException>(() =>
                    passwordService.CreatePasswordAsync("test@example.com", "invalidpassword", "123456"));
                
                Assert.Equal("Het wachtwoord moet minimaal 8 tekens lang zijn, minimaal één hoofdletter, één kleine letter en één cijfer bevatten.", exception.Message);

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
        
        [Fact]
        public async Task UpdatePasswordAsync_ShouldUpdateUserPasswordInContext()
        {
            // Arrange
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userService = new PasswordResetRepository(context); // Assuming you have an implementation of IUserService
                User user = new User { UserId = 1, PasswordHash = "oldHashedPassword", Email = "Blabla@blabla.nl" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                // Act
                await userService.UpdatePasswordAsync(user, "newPassword");

                // Assert
                User updatedUser = await context.Users.FindAsync(1);
                Assert.NotNull(updatedUser);

                // Verify that the password has been updated
                Assert.NotEqual("oldHashedPassword", updatedUser.PasswordHash);
                // Add more assertions based on your password hashing logic or behavior
            }
        }


        [Fact]
        public async Task UpdatePasswordAsync_InvalidUserId_ShouldNotUpdatePassword()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                PasswordResetRepository userService = new PasswordResetRepository(context); // Assuming you have an implementation of IUserService
                User user = new User { UserId = 1, PasswordHash = "oldHashedPassword", Email = "Blabla@blabla.nl" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                // Act
                await Assert.ThrowsAsync<NullReferenceException>(() =>
                    userService.UpdatePasswordAsync(new User { UserId = 2 }, "newPassword"));

                // Assert
                User unchangedUser = await context.Users.FindAsync(1);
                Assert.NotNull(unchangedUser);

                // Verify that the password has not been updated
                Assert.Equal("oldHashedPassword", unchangedUser.PasswordHash);
            }
        }

        
        
    }
}