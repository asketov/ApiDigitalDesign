using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.SubscribeModels
{
    public class SubscribeModel
    {
        public Guid SubscriberId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
