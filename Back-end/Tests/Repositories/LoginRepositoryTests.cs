using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;
using PL.Models.Requests;

namespace Tests.Repositories
{

    public class LoginRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CheckCredentials_ValidCredentials_ShouldReturnUser()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                UserRepository userRepository = new UserRepository(context);

                User user = new User
                {
                    UserId = 1,                    
                    Name = "test",
                    Email = "test@example.com",
                    PasswordHash = "zkiE/979pWcygZdGCnKiubLZywTAA0JJ6BeFfRSuw/Q=",
                    PasswordSalt = "Td2fD1/rI+0u0kt/RHSDMchDDJBX7EW6rmvUs7taLz4=" 
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                LoginRequestDTO loginRequest = new LoginRequestDTO
                {
                    Email = "test@example.com",
                    Password = "1" 
                };

                User result = await userRepository.CheckCredentials(loginRequest);

                Assert.NotNull(result);
                Assert.Equal(user.UserId, result.UserId);
            }
        }

        [Fact]
        public async Task CheckCredentials_InvalidEmail_ShouldThrowInvalidCredentialsException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                UserRepository userRepository = new UserRepository(context);

                LoginRequestDTO loginRequest = new LoginRequestDTO
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
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                UserRepository userRepository = new UserRepository(context);

                User user = new User
                {
                    UserId = 1,
                    Name = "test",
                    Email = "test@example.com",
                    PasswordHash = "hashed_password", 
                    PasswordSalt = "salt" 
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                LoginRequestDTO loginRequest = new LoginRequestDTO
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