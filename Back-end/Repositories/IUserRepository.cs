using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
        void Add(User entity);
        User checkCredentials(User user);
    }
}