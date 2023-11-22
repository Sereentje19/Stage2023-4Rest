using PL.Models;

namespace DAL.Repositories;

public interface IPasswordResetRepository
{
    Task PostResetCode(string code, string email);
    Task<string> CheckEnteredCode(string email, string code);
}