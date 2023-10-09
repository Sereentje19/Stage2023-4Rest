using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class UpdateCustomerFailedException : Exception
    {
        public UpdateCustomerFailedException(string message)
        : base(message)
        {

        }
    }
}