using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class CustomerAddException : Exception
    {
        public CustomerAddException(string message)
        : base(message)
        {

        }
    }
}