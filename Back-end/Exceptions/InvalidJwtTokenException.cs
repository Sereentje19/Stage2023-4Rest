using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class InvalidJwtTokenException : Exception
    {
        public InvalidJwtTokenException(string message)
        : base(message)
        {

        }
    }
}