using PL.Models.Requests;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models.Responses;

namespace BLL.Services
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
