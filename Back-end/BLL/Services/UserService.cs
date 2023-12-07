using DAL.Repositories;
using PL.Models.Requests;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Exceptions;
using PL.Models.Responses;

namespace BLL.Services
{
    public class UserService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public UserService(ILoginRepository loginRepository)
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
        
        public async Task PostUser(User user)
        {
             await _loginRepository.PostUser(user);
        }
        
        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            return await _loginRepository.GetAllUsers();
        }
        
        public async Task DeleteUser(string email)
        {
             await _loginRepository.DeleteUser(email);
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