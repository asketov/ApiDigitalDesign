using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.AutoMapper;
using AutoMapper;
using DAL.Entities;

namespace ApiDigitalDesign.Models.UserModels
{
    public class GetUserModel : IMapWith<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserModel>();
        }
    }
}
