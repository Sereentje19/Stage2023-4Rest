using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using DbContext = Stage4rest2023.Models.DbContext;


namespace Stage4rest2023.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<User> _dbSet;

        public LoginRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        /// <summary>
        /// Validates user credentials by checking the provided user's email and password against the repository.
        /// </summary>
        /// <param name="user">The user object containing email and password for validation.</param>
        /// <returns>The user object if valid credentials are found; otherwise, throws exceptions for incorrect email or password.</returns>
        /// <exception cref="Exception">Thrown when the email or password is incorrect.</exception>
        public async Task<User> CheckCredentials(LoginRequestDTO user)
        {
            User matchingUser = await _dbSet.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (matchingUser != null)
            {
                if (matchingUser.Password == user.Password)
                {
                    return matchingUser;
                }

                throw new InvalidCredentialsException("Wachtwoord is incorrect!");
            }

            throw new InvalidCredentialsException("Email is incorrect!");
        }
    }
}