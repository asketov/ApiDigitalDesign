using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;

namespace ApiDigitalDesign.Models.PostModels
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public List<AttachLinkModel> LinksToAttaches { get; set; } = new List<AttachLinkModel>();
        public DateTimeOffset Created { get; set; }
        public string? Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public int CountComments { get; set; }
        public int CountLikes { get; set; }
        
    }
}
