using PL.Models;
using PL.Models.Requests;

namespace BLL.Services;

public interface IPasswordResetService
{
    Task PostResetCode(string email);
    Task CheckEnteredCode(string email, string code);
    Task PostPassword(PasswordChangeRequest request);
}