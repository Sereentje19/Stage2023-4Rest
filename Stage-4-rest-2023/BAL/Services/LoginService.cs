using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Repositories;

namespace Stage4rest2023.Services
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
    }
}