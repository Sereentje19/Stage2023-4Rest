using DAL.Models;

namespace BLL.Interfaces
{
    public interface IJwtValidationService
    {
        string GenerateToken(User user);
    }
}
