using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message)
        : base(message)
        {

        }
    }
}