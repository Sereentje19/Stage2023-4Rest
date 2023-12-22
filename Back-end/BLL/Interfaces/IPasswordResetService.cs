using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace BLL.Interfaces;

public interface IPasswordResetService
{
    Task CreateResetCodeAsync(string email);
    Task CheckEnteredCodeAsync(string email, string code);
    Task CreatePasswordAsync(CreatePasswordRequestDto requestDto);
    Task UpdatePasswordAsync(UpdatePasswordRequestDto updatePasswordRequestDto);
}