using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class GetPostCommentsRequest
    {
        public Guid PostId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
