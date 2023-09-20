using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository ur)
        {
            _userRepository = ur;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User checkCredentials(User user)
        {
            return _userRepository.checkCredentials(user);
        }

        public void Post(User user)
        {
            _userRepository.Add(user);
        }

    }
}