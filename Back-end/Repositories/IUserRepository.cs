using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
        void Add(User entity);
        void Update(User entity);
        void Delete(User entity);
    }
}