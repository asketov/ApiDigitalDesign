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
    public class MetadataModel 
    {
        public Guid TempId { get; set; }
        public string Name { get; set; } = null!; 
        public string MimeType { get; set; } = null!; 
        public long Size { get; set; }

    }
}
