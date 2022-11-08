using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Posts
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException(string mes) : base(mes)
        {

        }
    }
}
