using DAL.Models;
using DAL.Models.Requests;

namespace BLL.Interfaces;

public interface IPasswordResetService
{
    Task CreateResetCodeAsync(string email);
    Task CheckEnteredCodeAsync(string email, string code);
    Task CreatePasswordAsync(PasswordChangeRequest request);
    Task UpdatePasswordAsync(User user, string password1, string password2, string password3);
}