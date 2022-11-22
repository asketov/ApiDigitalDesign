using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.LikeModels
{
    public class LikePostModel
    {
        public Guid? AuthorId { get; set; }
        public Guid PostId { get; set; }
    }
}
