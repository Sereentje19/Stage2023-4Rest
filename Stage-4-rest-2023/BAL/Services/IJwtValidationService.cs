using Microsoft.AspNetCore.Http;

namespace Stage4rest2023.Services
{
    public interface IJwtValidationService
    {
        string ValidateToken(HttpContext context);
        string GenerateToken();
    }
}