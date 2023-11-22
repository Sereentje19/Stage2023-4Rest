using PL.Models;

namespace BLL.Services;

public interface IPasswordResetService
{
    Task PostResetCode(string email);
    Task CheckEnteredCode(string email, string code);
    Task PostPassword(string email, string password1, string password2, string code);
}