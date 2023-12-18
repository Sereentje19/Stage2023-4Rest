using DAL.Models;
using DAL.Models.Requests;

namespace BLL.Interfaces;

public interface IPasswordResetService
{
    Task CreateResetCodeAsync(string email);
    Task CheckEnteredCodeAsync(string email, string code);
    Task CreatePasswordAsync(CreatePasswordRequestDto requestDto);
    Task UpdatePasswordAsync(UpdatePasswordRequestDto updatePasswordRequestDto);
}