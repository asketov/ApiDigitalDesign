using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.PostModels;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Comment, CommentModel>().ForMember(s => s.AuthorId, f => f.MapFrom(k => k.Author.Id));
        }
    }
}
