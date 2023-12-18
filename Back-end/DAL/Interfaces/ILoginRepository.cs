using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace DAL.Interfaces
{
    public interface ILoginRepository
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
        Task<User> GetUserByEmail(string email);
        Task PutUserEmail(User user, string email);
        Task PutUserName(User user);
        Task PostUser(User user);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task DeleteUser(string email);

    }
}
