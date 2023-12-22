using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            return await _userRepository.CheckCredentialsAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        
        public async Task CreateUserAsync(CreateUserRequestDto userRequest)
        {
             await _userRepository.CreateUserAsync(userRequest);
        }
        
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        
        public async Task DeleteUserAsync(string email)
        {
             await _userRepository.DeleteUserAsync(email);
        }

        public async Task UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            Console.WriteLine(updateUserRequestDto.Email1);
            Console.WriteLine(updateUserRequestDto.Email2);
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
                await _userRepository.UpdateUserNameAsync(user);
            }
            else
            {
                await _userRepository.UpdateUserEmailAsync(user, updateUserRequestDto.Email2);
            }
        }
    }
}