using DAL.Repositories;
using PL.Models.Requests;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Exceptions;

namespace BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        /// <summary>
        /// Checks the credentials of a user by delegating the task to the underlying login repository.
        /// </summary>
        /// <param name="user">The user whose credentials need to be checked.</param>
        /// <returns>
        /// The user object if the credentials are valid; otherwise, returns null.
        /// </returns>
        public async Task<User> CheckCredentials(LoginRequestDTO user)
        {
            return await _loginRepository.CheckCredentials(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _loginRepository.GetUserByEmail(email);
        }

        public async Task PutUser(User user, string email, bool updateName)
        {
            if (updateName)
            {
                await _loginRepository.PutUserName(user);
            }
            else
            {
                await _loginRepository.PutUserEmail(user, email);
            }
        }
    }
}