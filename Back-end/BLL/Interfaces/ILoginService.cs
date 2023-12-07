using PL.Models.Requests;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ILoginService
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
        Task<User> GetUserByEmail(string email);
        Task PutUser(User user, string email, bool updateName);
    }
}
