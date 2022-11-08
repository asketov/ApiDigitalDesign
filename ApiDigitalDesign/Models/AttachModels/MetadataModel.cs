using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.AutoMapper;
using ApiDigitalDesign.Services;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.Models.AttachModels
{
    public class MetadataModel : IMapWith<PostAttach>
    {
        public Guid TempId { get; set; }
        public string Name { get; set; } = null!; 
        public string MimeType { get; set; } = null!; 
        public long Size { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.AttachModels.MetadataModel, DAL.Entities.PostAttach>()
                .ForMember(d => d.FilePath, m => m.MapFrom(s => AttachService.CopyTempFileToAttaches(s.TempId)))
                .ForMember(d=>d.Id, m=> m.MapFrom(s => Guid.NewGuid()));
        }
    }
}
