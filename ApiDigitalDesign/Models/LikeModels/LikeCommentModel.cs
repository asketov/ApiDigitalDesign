using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.LikeModels
{
    public class LikeCommentModel
    {
        public Guid? AuthorId { get; set; }
        public Guid CommentId { get; set; }
    }
}
