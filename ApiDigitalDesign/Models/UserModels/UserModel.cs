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
    public class UserModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTimeOffset BirthDate { get; set; }
        public bool CloseAccount { get; set; }
    }
}
