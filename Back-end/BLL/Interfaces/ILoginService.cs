using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace BLL.Interfaces
{
    public interface ILoginService
    {
        Task<User> CheckCredentialsAsync(LoginRequestDTO user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user, string email, bool updateName);
        Task CreateUserAsync(User user);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task DeleteUserAsync(string email);
    }
}
