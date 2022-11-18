using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.AutoMapper;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.Models.PostModels
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Content { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
        
    }
}
