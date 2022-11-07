using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual User Author { get; set; }
        public virtual Post Post { get; set; }



    }
}
