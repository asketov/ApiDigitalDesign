using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CreateCommentModel
    {
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public Guid PostId { get; set; }
    }
}
