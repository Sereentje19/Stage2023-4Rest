using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Services
{
    public class UserService
    {
        private UserRepository _userRepository = new UserRepository();
        public string GetName()
        {
            return _userRepository.GetName();
        }
    }
}