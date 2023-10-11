using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Exceptions
{
    public class UpdateDocumentFailedException : Exception
    {
        public UpdateDocumentFailedException(string message)
        : base(message)
        {

        }
    }
}