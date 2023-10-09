using Back_end.Models;
using Back_end.Models.DTOs;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        /// <summary>
        /// Initializes a new instance of the LoginService class with the provided login repository.
        /// </summary>
        /// <param name="lr">The login repository responsible for database interactions.</param>
        public LoginService(ILoginRepository lr)
        {
            _loginRepository = lr;
        }

        /// <summary>
        /// Checks the credentials of a user by delegating the task to the underlying login repository.
        /// </summary>
        /// <param name="user">The user whose credentials need to be checked.</param>
        /// <returns>
        /// The user object if the credentials are valid; otherwise, returns null.
        /// </returns>
        public User checkCredentials(LoginRequestDTO user)
        {
            return _loginRepository.checkCredentials(user);
        }
    }
}