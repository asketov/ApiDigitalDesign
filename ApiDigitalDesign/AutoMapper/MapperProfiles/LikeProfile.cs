using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.LikeModels;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<LikeCommentModel, CommentLike>()
                .ForMember(f=>f.Created, k=>k.MapFrom(f => DateTime.UtcNow));
            CreateMap<LikePostModel, PostLike>()
                .ForMember(f => f.Created, k => k.MapFrom(f => DateTime.UtcNow));
        }
    }
}
