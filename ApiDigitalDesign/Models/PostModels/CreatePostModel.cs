using ApiDigitalDesign.Models.AttachModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CreatePostModel
    {
        public List<MetadataModel> Attaches { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Title { get; set; } = null!;
    }
}
