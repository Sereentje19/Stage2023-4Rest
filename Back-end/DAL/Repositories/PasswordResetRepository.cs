using System.Security.Cryptography;
using System.Text;
using PL.Exceptions;
using PL.Models.Requests;
using PL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{

    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PasswordResetCode> _dbSet;

        public PasswordResetRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<PasswordResetCode>();
        }

        /// <summary>
        /// Posts a password reset code for a user and sets its expiration time.
        /// </summary>
        /// <param name="code">The reset code to associate with the user.</param>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>
        /// The User object associated with the provided email address.
        /// </returns>
        public async Task<User> PostResetCode(string code, string email)
        {
            User user = await _context.Users
                .Where(u => u.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

            if (user== null)
            {
                throw new InvalidCredentialsException("Geen gebruiker gevonden");
            }

            PasswordResetCode prc = new PasswordResetCode
            {
                UserId = user.UserId,
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5)
            };

            await _dbSet.AddAsync(prc);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Checks the validity of the entered reset code and retrieves the associated user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="code">The reset code entered by the user.</param>
        /// <returns>
        /// The User object associated with the provided email address and valid reset code.
        /// </returns>
        public async Task<User> CheckEnteredCode(string email, string code)
        {
            var userWithCode = await _context.Users
                .Join(_dbSet,
                    user => user.UserId,
                    prc => prc.UserId,
                    (user, prc) => new { User = user, Prc = prc })
                .FirstOrDefaultAsync(joined =>
                    joined.User.Email == email &&
                    joined.Prc.Code == code &&
                    joined.Prc.ExpirationTime > DateTime.UtcNow);

            if (userWithCode == null)
            {
                throw new InputValidationException("Sessie verlopen of ongeldige code. Probeer het opnieuw.");
            }

            return userWithCode.User;
        }

        /// <summary>
        /// Posts a new password for a user after the password recovery process.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The new password to be hashed and saved.</param>
        /// <param name="code">The reset code entered by the user.</param>
        public async Task PostPassword(string email, string password, string code)
        {
            User user = await CheckEnteredCode(email, code);
            await HashAndSavePassword(user, password);
        }

        /// <summary>
        /// Hashes and saves the provided password for a user.
        /// </summary>
        /// <param name="user">The User object for whom the password is to be hashed and saved.</param>
        /// <param name="password">The new password to be hashed.</param>
        private async Task HashAndSavePassword(User user, string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 32, 10000))
            {
                byte[] hashedBytes = deriveBytes.GetBytes(32);
                user.PasswordHash = Convert.ToBase64String(hashedBytes);
                user.PasswordSalt = Convert.ToBase64String(deriveBytes.Salt);
            }

            await _context.SaveChangesAsync();
        }
    }
}