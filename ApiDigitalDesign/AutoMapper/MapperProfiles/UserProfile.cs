using ApiDigitalDesign.AutoMapper.MapperActions;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.UserModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Helpers;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Avatar, AttachModel>();
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime));
            CreateMap<User, UserAvatarModel>().AfterMap<AvatarMapperAction>();
        }
    }
}
