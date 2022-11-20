using ApiDigitalDesign.Models.AuthModels;
using ApiDigitalDesign.Models.UserModels;
using AutoMapper;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<SignInRequest, SignInModel>();
            CreateMap<CreateUserModel, SignInModel>();
        }
    }
}
