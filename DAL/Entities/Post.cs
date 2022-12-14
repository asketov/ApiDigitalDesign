using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual ICollection<PostAttach> PostAttaches { get; set; } = new List<PostAttach>();
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<PostLike>? Likes { get; set; }
    }
}
