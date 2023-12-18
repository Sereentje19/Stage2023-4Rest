using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace BLL.Interfaces
{
    public interface ILoginService
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
        Task<User> GetUserByEmail(string email);
        Task PutUser(User user, string email, bool updateName);
        Task PostUser(User user);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task DeleteUser(string email);
    }
}
