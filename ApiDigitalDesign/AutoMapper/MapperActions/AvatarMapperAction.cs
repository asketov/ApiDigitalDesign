using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.AutoMapper.MapperActions
{
    public class AvatarMapperAction : IMappingAction<User, UserAvatarModel>
    {
        private LinkGeneratorService _links;

        public AvatarMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }

        public void Process(User source, UserAvatarModel destination, ResolutionContext context)
        {
            _links.LinkToAvatar(source, destination);
        }
    }
}
