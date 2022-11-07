using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.AttachModels
{
    public class MetadataModel
    {
        public Guid TempId { get; set; }
        public string Name { get; set; } 
        public string MimeType { get; set; } 
        public long Size { get; set; }
    }
}
