using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public Guid PostId { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? Id { get; set; }
    }
}
