using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ILoginService
    {
        User checkCredentials(LoginRequestDTO user);
    }
}