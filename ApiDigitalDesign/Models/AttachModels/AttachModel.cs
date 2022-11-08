using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.AutoMapper;
using ApiDigitalDesign.Services;
using AutoMapper;
using Common.Helpers;
using DAL.Entities;

namespace ApiDigitalDesign.Models.AttachModels
{
    public class AttachModel : IMapWith<Avatar>, IMapWith<Attach>
    {
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DAL.Entities.Avatar, Models.AttachModels.AttachModel>();
            profile.CreateMap<DAL.Entities.Attach, Models.AttachModels.AttachModel>();

        }
    }
}
