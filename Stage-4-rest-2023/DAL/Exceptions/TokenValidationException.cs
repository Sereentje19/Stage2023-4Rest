using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage4rest2023.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message)
        : base(message)
        {

        }
    }
}