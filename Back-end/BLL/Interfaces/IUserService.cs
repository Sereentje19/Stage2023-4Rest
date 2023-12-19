using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> CheckCredentialsAsync(LoginRequestDto user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto);
        Task CreateUserAsync(CreateUserRequestDto userRequest);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task DeleteUserAsync(string email);
    }
}
