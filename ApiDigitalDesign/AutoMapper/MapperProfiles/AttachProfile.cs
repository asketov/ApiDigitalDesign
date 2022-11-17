using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;
using AutoMapper;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class AttachProfile : Profile
    {
        public AttachProfile()
        {
            CreateMap<DAL.Entities.Avatar, Models.AttachModels.AttachModel>();
            CreateMap<DAL.Entities.Attach, Models.AttachModels.AttachModel>();
            CreateMap<MetadataModel, MetadataPathModel>();
        }
    }
}
