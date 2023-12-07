using System.Security.Cryptography;
using PL.Exceptions;
using PL.Models.Requests;
using PL.Models;
using Microsoft.EntityFrameworkCore;
using PL.Models.Responses;

namespace DAL.Repositories
{
    public class UserRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbSet
                .SingleOrDefaultAsync(l => l.Email == email);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            return await _dbSet
                .Select(user => new UserResponse
                {
                    Email = user.Email,
                    Name = user.Name
                }).ToListAsync();
        }

        public async Task PutUserEmail(User user, string email)
        {
            User existingUser = await _dbSet
                .SingleOrDefaultAsync(l => l.UserId == user.UserId);

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InputValidationException("Email veld is leeg.");
            }

            if (user.Email != email)
            {
                throw new InputValidationException("Email velden zijn niet gelijk aan elkaar.");
            }

            if (!user.Email.Contains('@'))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            existingUser.Email = user.Email;
            _dbSet.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task PutUserName(User user)
        {
            User existingUser = await _dbSet
                .SingleOrDefaultAsync(l => l.UserId == user.UserId);

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new InputValidationException("Naam veld is leeg.");
            }

            existingUser.Name = user.Name;
            _dbSet.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task PostUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new InputValidationException("Naam veld is leeg.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InputValidationException("Email veld is leeg.");
            }

            if (!user.Email.Contains('@'))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            _dbSet.Add(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteUser(string email)
        {
            User user = await _dbSet
                .SingleOrDefaultAsync(l => l.Email == email);
            
            _dbSet.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Validates user credentials by checking the provided user's email and password against the repository.
        /// </summary>
        /// <param name="user">The user object containing email and password for validation.</param>
        /// <returns>The user object if valid credentials are found; otherwise, throws exceptions for incorrect email or password.</returns>
        /// <exception cref="Exception">Thrown when the email or password is incorrect.</exception>
        public async Task<User> CheckCredentials(LoginRequestDTO user)
        {
            User matchingUser = await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());

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
        private static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            using (Rfc2898DeriveBytes deriveBytes =
                   new Rfc2898DeriveBytes(enteredPassword, Convert.FromBase64String(storedSalt), 10000))
            {
                byte[] enteredPasswordHash = deriveBytes.GetBytes(32);
                string enteredPasswordHashString = Convert.ToBase64String(enteredPasswordHash);

                return string.Equals(enteredPasswordHashString, storedHash);
            }
        }
    }
}