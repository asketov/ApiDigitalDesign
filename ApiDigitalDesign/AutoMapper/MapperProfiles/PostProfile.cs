using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.AutoMapper.MapperActions;
using ApiDigitalDesign.Models.AttachModels;
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
            CreateMap<Post, PostModel>().ForMember(s => s.AuthorId, f => f.MapFrom(k => k.Author.Id))
                .ForMember(d => d.LinksToAttaches, m => m.MapFrom(d => d.PostAttaches)); 
            CreateMap<PostAttach, AttachLinkModel>().AfterMap<PostAttachMapperAction>();
            CreateMap<PostAttach, AttachModel>();
            CreateMap<CreatePostRequest, CreatePostModel>();
            CreateMap<MetadataModel, MetadataPathModel>();
            CreateMap<MetadataPathModel, PostAttach>();
            CreateMap<CreatePostModel, Post>()
                .ForMember(d => d.PostAttaches, m => m.MapFrom(s => s.Attaches))
                .ForMember(d => d.Created, m => m.MapFrom(s => DateTime.UtcNow))
                .ForMember(d => d.Id, s => s.MapFrom(s => Guid.NewGuid()));


        }
    }
}
