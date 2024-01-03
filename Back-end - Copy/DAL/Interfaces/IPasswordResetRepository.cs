using DAL.Models;

namespace DAL.Interfaces;

public interface IPasswordResetRepository
{
    Task<User> CreateResetCodeAsync(string code, string email);
    Task<User> CheckEnteredCodeAsync(string email, string code);
    Task CreatePasswordAsync(string email, string password, string code);
    Task UpdatePasswordAsync(User user, string password);
}