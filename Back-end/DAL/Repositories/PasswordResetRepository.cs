using System.Security.Cryptography;
using System.Text;
using PL.Exceptions;
using PL.Models.Requests;
using PL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<PasswordResetCode> _dbSet;

    public PasswordResetRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<PasswordResetCode>();
    }

    public async Task PostResetCode(string code, string email)
    {
        int userId = await _context.Users
            .Where(u => u.Email.ToLower() == email.ToLower())
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

        if (userId == 0)
        {
            throw new InvalidCredentialsException("Geen gebruiker gevonden");
        }

        PasswordResetCode prc = new PasswordResetCode
        {
            UserId = userId,
            Code = code,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5)
        };

        await _dbSet.AddAsync(prc);
        await _context.SaveChangesAsync();
    }

    public async Task<string> CheckEnteredCode(string email, string code)
    {
        User matchingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);


        if (matchingUser != null)
        {
            PasswordResetCode prc = await _dbSet.FirstOrDefaultAsync(u =>
                u.UserId == matchingUser.UserId &&
                u.Code == code &&
                u.ExpirationTime > DateTime.UtcNow);


            if (prc != null)
            {
                string GeneratedPassword = GenerateRandomPassword(12);
                await HashAndSavePassword(matchingUser, GeneratedPassword);
                return GeneratedPassword;
            }
        }

        throw new InvalidCredentialsException("Code is incorrect of verlopen.");
    }

    private async Task HashAndSavePassword(User user, string password)
    {
        // Hash the password
        using (var deriveBytes = new Rfc2898DeriveBytes(password, 32, 10000))
        {
            byte[] hashedBytes = deriveBytes.GetBytes(32); // 32 bytes for a 256-bit key
            user.PasswordHash = Convert.ToBase64String(hashedBytes);
            user.PasswordSalt = Convert.ToBase64String(deriveBytes.Salt);
        }

        await _context.SaveChangesAsync();
    }

    public static string GenerateRandomPassword(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            StringBuilder password = new StringBuilder(length);

            foreach (byte b in randomBytes)
            {
                password.Append(validChars[b % validChars.Length]);
            }

            return password.ToString();
        }
    }
}