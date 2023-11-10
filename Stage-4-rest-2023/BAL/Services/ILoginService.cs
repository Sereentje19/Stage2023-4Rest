using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface ILoginService
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
    }
}