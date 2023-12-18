using DAL.Models;
using DAL.Models.Requests;

namespace BLL.Interfaces;

public interface IPasswordResetService
{
    Task PostResetCode(string email);
    Task CheckEnteredCode(string email, string code);
    Task PostPassword(PasswordChangeRequest request);
    Task PutPassword(User user, string password1, string password2, string password3);
}