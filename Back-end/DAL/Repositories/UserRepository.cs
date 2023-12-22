using System.Security.Cryptography;
using Azure.Core;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A collection of user objects.</returns>
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _dbSet
                .Select(user => new UserResponseDto
                {
                    Email = user.Email,
                    Name = user.Name
                })
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a user from the database based on their email.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user object matching the provided email.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .SingleOrDefaultAsync(l => l.Email == email);
        }
        
        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="userRequestDto">The data required to create a new user.</param>
        public async Task CreateUserAsync(CreateUserRequestDto userRequestDto)
        {
            User existingUser = await _dbSet
                .SingleOrDefaultAsync(l => l.Email == userRequestDto.Email);

            if (existingUser != null)
            {
                throw new InputValidationException("Email bestaat al.");
            }

            if (string.IsNullOrWhiteSpace(userRequestDto.Name))
            {
                throw new InputValidationException("Naam veld is leeg.");
            }

            if (string.IsNullOrWhiteSpace(userRequestDto.Email))
            {
                throw new InputValidationException("Email veld is leeg.");
            }

            if (!userRequestDto.Email.Contains('@'))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            User user = new User()
            {
                Email = userRequestDto.Email,
                Name = userRequestDto.Name
            };

            _dbSet.Add(user);
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Updates the email address of a user in the database.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        /// <param name="email">The new email address for the user.</param>
        public async Task UpdateUserEmailAsync(User user, string email)
        {
            await CheckEmailExistsAsync(user);

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

        /// <summary>
        /// Updates the name of a user in the database.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        public async Task UpdateUserNameAsync(User user)
        {
            await CheckEmailExistsAsync(user);

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

        /// <summary>
        /// Deletes a user from the database based on their email.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        public async Task DeleteUserAsync(string email)
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
        public async Task<User> CheckCredentialsAsync(LoginRequestDto user)
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

        /// <summary>
        /// Checks if an email already exists for a user in the database.
        /// </summary>
        /// <param name="user">The user object containing the email to check.</param>
        /// <exception cref="InputValidationException">Thrown when the email already exists.</exception>
        private async Task CheckEmailExistsAsync(User user)
        {
            User existingUserEmail = await _dbSet
                .SingleOrDefaultAsync(l => l.Email == user.Email);

            if (existingUserEmail != null)
            {
                throw new InputValidationException("Email bestaat al.");
            }
        }

    }
}