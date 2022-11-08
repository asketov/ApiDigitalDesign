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
    public class CommentModel : IMapWith<Comment>
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Content { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Comment, CommentModel>().ForMember(s => s.AuthorId, f => f.MapFrom(k => k.Author.Id));
        }
    }
}
