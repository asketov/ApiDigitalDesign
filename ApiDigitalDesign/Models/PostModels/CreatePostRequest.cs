using ApiDigitalDesign.Attributes;
using ApiDigitalDesign.Models.AttachModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CreatePostRequest
    {
        [Required] [NotEmpty] 
        public List<MetadataModel> Attaches { get; set; } = new List<MetadataModel>();
        [MaxLength(2000)]
        public string? Title { get; set; } = null!;
        public Guid? AuthorId { get; set; }
    }
}
