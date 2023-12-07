using PL.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;
using PL.Models.Responses;

namespace DAL.Repositories
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
