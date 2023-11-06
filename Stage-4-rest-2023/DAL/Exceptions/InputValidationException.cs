using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage4rest2023.Exceptions
{
    public class InputValidationException : Exception
    {
        public InputValidationException(string message)
        : base(message)
        {

        }
    }
}