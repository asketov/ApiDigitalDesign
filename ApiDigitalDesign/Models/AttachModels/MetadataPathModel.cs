using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.AttachModels
{
    public class MetadataPathModel : MetadataModel
    {
        public string FilePath { get; set; } = null!;
        public Guid AuthorId { get; set; }
    }
}
