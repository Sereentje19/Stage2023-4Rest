using Microsoft.AspNetCore.Http;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface IJwtValidationService
    {
        // string ValidateToken(HttpContext context);
        string GenerateToken(LoginRequestDTO user);
    }
}