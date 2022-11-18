using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.User
{
    public class AvatarNotFoundException : Exception
    {
        public AvatarNotFoundException(string mess) : base(mess)
        {

        }
    }
}
