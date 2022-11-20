using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.SubscribeModels;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.AutoMapper.MapperProfiles
{
    public class SubscribeProfile : Profile
    {
        public SubscribeProfile()
        {
            CreateMap<AddSubscribeRequest, AddSubscribeModel>()
                .ForMember(u => u.Created, s => s.MapFrom(k => DateTime.UtcNow))
                .ForMember(u => u.IsAccepted, k => k.MapFrom(f => true));
            CreateMap<AddSubscribeModel, Subscribe>();
        }
    }
}
