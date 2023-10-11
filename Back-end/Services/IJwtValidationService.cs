namespace Back_end.Services
{
    public interface IJwtValidationService
    {
        string ValidateToken(HttpContext context);
        string GenerateToken();
    }
}