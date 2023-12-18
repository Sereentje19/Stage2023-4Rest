using DAL.Models;

namespace DAL.Interfaces;

public interface IPasswordResetRepository
{
    Task<User> PostResetCode(string code, string email);
    Task<User> CheckEnteredCode(string email, string code);
    Task PostPassword(string email, string password, string code);
    Task PutPassword(User user, string password);
}