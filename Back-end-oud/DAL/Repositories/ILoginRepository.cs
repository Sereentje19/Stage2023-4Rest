using PL.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
{
    public interface ILoginRepository
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
    }
}
