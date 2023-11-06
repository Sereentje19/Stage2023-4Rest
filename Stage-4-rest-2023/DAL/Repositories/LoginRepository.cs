using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Microsoft.EntityFrameworkCore;


namespace Stage4rest2023.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<User> _dbSet;

        /// <summary>
        /// Initializes a new instance of the LoginRepository class with the provided NotificationContext.
        /// </summary>
        /// <param name="context">The NotificationContext used for database interactions.</param>
        public LoginRepository(NotificationContext context)
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
        public User CheckCredentials(LoginRequestDTO user)
        {
            User matchingUser = _dbSet.FirstOrDefault(u => u.Email == user.Email);

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