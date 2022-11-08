using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class PostModel
    {
        public List<string> LinksToAttaches { get; set; } = new List<string>();
        public DateTimeOffset Created { get; set; }
        public string Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
    }
}
