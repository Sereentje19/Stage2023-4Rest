using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace DAL.Interfaces
{
    public interface ILoginRepository
    {
        Task<User> CheckCredentialsAsync(LoginRequestDTO user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserEmailAsync(User user, string email);
        Task UpdateUserNameAsync(User user);
        Task CreateUserAsync(User user);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task DeleteUserAsync(string email);

    }
}
