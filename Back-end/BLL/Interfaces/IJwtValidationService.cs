using PL.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace BLL.Services
{
    public interface IJwtValidationService
    {
        string GenerateToken(User user);
    }
}
