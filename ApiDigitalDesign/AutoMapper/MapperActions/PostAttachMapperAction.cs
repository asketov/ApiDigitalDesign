using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Services;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.AutoMapper.MapperActions
{
    public class PostAttachMapperAction : IMappingAction<PostAttach, AttachLinkModel>
    {
        private LinkGeneratorService _links;
        public PostAttachMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(PostAttach source, AttachLinkModel destination, ResolutionContext context)
        {
            _links.LinkToContent(source, destination);
        }
    }
}
