using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
{
    public interface ILoginRepository
    {
        Task<User> CheckCredentials(LoginRequestDTO user);
    }
}