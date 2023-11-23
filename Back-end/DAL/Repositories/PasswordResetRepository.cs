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

    public async Task<User> PostResetCode(string code, string email)
    {
        User user = await _context.Users
            .Where(u => u.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync();

        if (user.UserId == 0)
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

    public async Task PostPassword(string email, string password, string code)
    {
        User user = await CheckEnteredCode(email, code);
        await HashAndSavePassword(user, password);
    }

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