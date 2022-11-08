using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Auth
{
    /// <summary>
    /// Token in not valid
    /// </summary>
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message)
        {

        }
    }
}
