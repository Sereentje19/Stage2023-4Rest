using PL.Models;

namespace BLL.Services;

public interface IPasswordResetService
{
    Task PostResetCode(string email);
    Task CheckEnteredCode(string email, string code);
}