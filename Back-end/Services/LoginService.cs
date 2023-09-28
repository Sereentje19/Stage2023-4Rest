using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository ur)
        {
            _loginRepository = ur;
        }

        public User checkCredentials(User user)
        {
            return _loginRepository.checkCredentials(user);
        }
    }
}