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
        public async Task<User> CheckCredentialsAsync(LoginRequestDto user)
        {
            return await _loginRepository.CheckCredentialsAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _loginRepository.GetUserByEmailAsync(email);
        }
        
        public async Task CreateUserAsync(CreateUserRequestDto userRequest)
        {
             await _loginRepository.CreateUserAsync(userRequest);
        }
        
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _loginRepository.GetAllUsersAsync();
        }
        
        public async Task DeleteUserAsync(string email)
        {
             await _loginRepository.DeleteUserAsync(email);
        }

        public async Task UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            User user = new User()
            {
                Email = updateUserRequestDto.Email1,
                Name = updateUserRequestDto.Name,
                UserId = updateUserRequestDto.UserId,
                PasswordHash = updateUserRequestDto.PasswordHash,
                PasswordSalt = updateUserRequestDto.PasswordSalt
            };
            
            if (updateUserRequestDto.UpdateName)
            {
                await _loginRepository.UpdateUserNameAsync(user);
            }
            else
            {
                await _loginRepository.UpdateUserEmailAsync(user, updateUserRequestDto.Email2);
            }
        }
    }
}