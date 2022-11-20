using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ApiDigitalDesign.Models.SubscribeModels
{
    public class AddSubscribeRequest
    {
        public Guid? SubscriberId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
