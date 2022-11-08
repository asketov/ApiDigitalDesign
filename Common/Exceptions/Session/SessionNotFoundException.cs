using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Session
{
    public class SessionNotFoundException : Exception
    {
        public SessionNotFoundException(string message) : base(message)
        {

        }
    }
}
