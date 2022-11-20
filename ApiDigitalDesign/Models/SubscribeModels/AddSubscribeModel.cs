using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.SubscribeModels
{
    public class AddSubscribeModel
    {
        public Guid SubscriberId { get; set; }
        public Guid RecipientId { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsAccepted { get; set; }
    }
}
