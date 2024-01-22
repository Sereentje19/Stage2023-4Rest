using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CheckCredentialsAsync(LoginRequestDto user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserEmailAsync(User user, string email);
        Task UpdateUserNameAsync(User user);
        Task CreateUserAsync(User userRequest);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task DeleteUserAsync(string email);

    }
}
