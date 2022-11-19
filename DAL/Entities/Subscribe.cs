using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Subscribe
    {
        public User Subscriber { get; set; } = null!;
        public User Recipient { get; set; } = null!;
        public Guid SubscriberId { get; set; }
        public Guid RecipientId { get; set; }
        public bool IsAccepted { get; set; }
        public DateTimeOffset Created { get; set; }

    }
}
