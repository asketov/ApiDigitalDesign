using ApiDigitalDesign.Models.AttachModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Attributes;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CreatePostModel
    {
        public List<MetadataPathModel> Attaches { get; set; } = new List<MetadataPathModel>();
        public string? Title { get; set; } = null!;
        public Guid AuthorId { get; set; } 
        public Guid Id { get; set; }

    }
}
