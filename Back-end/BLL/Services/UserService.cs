using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

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
        public async Task<User> CheckCredentialsAsync(LoginRequestDTO user)
        {
            return await _loginRepository.CheckCredentialsAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _loginRepository.GetUserByEmailAsync(email);
        }
        
        public async Task CreateUserAsync(User user)
        {
             await _loginRepository.CreateUserAsync(user);
        }
        
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            return await _loginRepository.GetAllUsersAsync();
        }
        
        public async Task DeleteUserAsync(string email)
        {
             await _loginRepository.DeleteUserAsync(email);
        }

        public async Task UpdateUserAsync(User user, string email, bool updateName)
        {
            if (updateName)
            {
                await _loginRepository.UpdateUserNameAsync(user);
            }
            else
            {
                await _loginRepository.UpdateUserEmailAsync(user, email);
            }
        }
    }
}