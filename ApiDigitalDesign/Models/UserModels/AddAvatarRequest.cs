using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;

namespace ApiDigitalDesign.Models.UserModels
{
    public class AddAvatarRequest
    {
        public Guid? Id { get; set; }
        [Required] public MetadataModel Avatar { get; set; } = null!;
        public Guid? UserId { get; set; }
    }
}
