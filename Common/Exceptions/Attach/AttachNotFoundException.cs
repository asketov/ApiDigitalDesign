using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Attach
{
    public class AttachNotFoundException : Exception
    {
        public AttachNotFoundException(string message) : base(message)
        {

        }
    }
}
