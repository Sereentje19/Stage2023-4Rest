using System.Security.Cryptography;
using PL.Exceptions;
using PL.Models.Requests;
using PL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public LoginRepository(ApplicationDbContext context)
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
                if (VerifyPassword(user.Password, matchingUser.PasswordHash, matchingUser.PasswordSalt))
                {
                    return matchingUser;
                }

                throw new InvalidCredentialsException("Wachtwoord is incorrect!");
            }

            throw new InvalidCredentialsException("Email is incorrect!");
        }
        
        /// <summary>
        /// Verifies the correctness of a password by comparing the entered password hash with the stored hash.
        /// </summary>
        /// <param name="enteredPassword">The password entered by the user.</param>
        /// <param name="storedHash">The stored password hash to compare against.</param>
        /// <param name="storedSalt">The stored salt value used during password hashing.</param>
        /// <returns>
        /// True if the entered password hash matches the stored hash; otherwise, false.
        /// </returns>
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(enteredPassword, Convert.FromBase64String(storedSalt), 10000))
            {
                byte[] enteredPasswordHash = deriveBytes.GetBytes(32); 
                string enteredPasswordHashString = Convert.ToBase64String(enteredPasswordHash);

                return string.Equals(enteredPasswordHashString, storedHash);
            }
        }

    }
}
