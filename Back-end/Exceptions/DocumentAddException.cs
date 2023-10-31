using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class DocumentAddException : Exception
    {
        public DocumentAddException(string message)
        : base(message)
        {

        }
    }
}