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
using ApiDigitalDesign.Models.AuthModels;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserModel, SignInModel>();
            CreateMap<User, UserModel>();
            CreateMap<Avatar, AttachModel>();
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                .ForMember(d => d.CloseAccount, m => m.MapFrom(s => false))
                .ForMember(d => d.Deleted, m => m.MapFrom(s => false));

            CreateMap<User, UserAvatarModel>().AfterMap<AvatarMapperAction>();
            CreateMap<SignInRequest, SignInModel>();
        }
    }
}
