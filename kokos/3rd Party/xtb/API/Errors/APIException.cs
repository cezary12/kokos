using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Errors
{
    /// <summary>
    /// Base class for all API exceptions.
    /// </summary>
    public class APIException : Exception
    {
        public APIException()
            : base()
        {
        }

        public APIException(string message)
            : base(message)
        {
        }
    }
}
