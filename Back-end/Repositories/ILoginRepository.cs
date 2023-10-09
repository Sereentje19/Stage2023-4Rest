using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface ILoginRepository
    {
        User checkCredentials(LoginRequestDTO user);
    }
}