using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Services
{
    public interface IJwtValidationService
    {
        string ValidateToken(HttpContext context);
        string GenerateToken();
    }
}