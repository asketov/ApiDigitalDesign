using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Attach
{
    public class DirectoryNotExistException : Exception
    {
        public DirectoryNotExistException(string message) : base(message)
        {

        }
    }
}
