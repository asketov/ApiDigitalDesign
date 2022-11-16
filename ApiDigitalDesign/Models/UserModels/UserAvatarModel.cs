using ApiDigitalDesign.AutoMapper;
using ApiDigitalDesign.AutoMapper.MapperActions;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.Models.UserModels
{
    public class UserAvatarModel : UserModel
    {
        public string? AvatarLink { get; set; }

    }
}
