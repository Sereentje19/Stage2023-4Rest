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

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user with the specified email address.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        
        /// <summary>
        /// Creates a new user based on the provided user request.
        /// </summary>
        /// <param name="userRequest">The user request information used to create the new user.</param>
        public async Task CreateUserAsync(CreateUserRequestDto userRequest)
        {
             await _userRepository.CreateUserAsync(userRequest);
        }
        
        /// <summary>
        /// Retrieves a collection of user response DTOs representing all users.
        /// </summary>
        /// <returns>A collection of user response DTOs.</returns>
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        
        /// <summary>
        /// Deletes a user with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        public async Task DeleteUserAsync(string email)
        {
             await _userRepository.DeleteUserAsync(email);
        }

        /// <summary>
        /// Updates a user's name or email based on the provided update request.
        /// </summary>
        /// <param name="updateUserRequestDto">The update request containing information about the user update.</param>
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