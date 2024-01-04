using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Exceptions;
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
        public Task<User> CheckCredentialsAsync(LoginRequestDto user)
        {
            return _userRepository.CheckCredentialsAsync(user);
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user with the specified email address.</returns>
        public Task<User> GetUserByEmailAsync(string email)
        {
            return _userRepository.GetUserByEmailAsync(email);
        }
        
        /// <summary>
        /// Creates a new user based on the provided user request.
        /// </summary>
        /// <param name="userRequest">The user request information used to create the new user.</param>
        public Task CreateUserAsync(CreateUserRequestDto userRequest)
        {
            ValidationHelper.ValidateObject(userRequest);
            
            if (string.IsNullOrWhiteSpace(userRequest.Name))
            {
                throw new InputValidationException("Naam veld is leeg.");
            }

            if (string.IsNullOrWhiteSpace(userRequest.Email))
            {
                throw new InputValidationException("Email veld is leeg.");
            }

            if (!userRequest.Email.Contains('@'))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            User user = new User()
            {
                Email = userRequest.Email,
                Name = userRequest.Name
            };
            
            return _userRepository.CreateUserAsync(user);
        }
        
        /// <summary>
        /// Retrieves a collection of user response DTOs representing all users.
        /// </summary>
        /// <returns>A collection of user response DTOs.</returns>
        public Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }
        
        /// <summary>
        /// Deletes a user with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        public Task DeleteUserAsync(string email)
        {
            return _userRepository.DeleteUserAsync(email);
        }

        /// <summary>
        /// Updates a user's name or email based on the provided update request.
        /// </summary>
        /// <param name="updateUserRequestDto">The update request containing information about the user update.</param>
        public Task UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            ValidationHelper.ValidateObject(updateUserRequestDto);
            
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
                if (string.IsNullOrWhiteSpace(user.Name))
                {
                    throw new InputValidationException("Naam veld is leeg.");
                }
                
                return _userRepository.UpdateUserNameAsync(user);
            }

            CheckInputfields(user, updateUserRequestDto);
            
            return _userRepository.UpdateUserEmailAsync(user, updateUserRequestDto.Email2);
        }

        private static void CheckInputfields(User user,UpdateUserRequestDto updateUserRequestDto)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InputValidationException("Email veld is leeg.");
            }

            if (user.Email != updateUserRequestDto.Email2)
            {
                throw new InputValidationException("Email velden zijn niet gelijk aan elkaar.");
            }

            if (!user.Email.Contains('@'))
            {
                throw new InputValidationException("Geen geldige email.");
            }
        }
    }
}