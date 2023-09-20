using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Post(User user);
        User checkCredentials(User user);
    }
}