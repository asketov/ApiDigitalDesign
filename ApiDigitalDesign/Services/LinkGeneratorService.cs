using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.PostModels;
using ApiDigitalDesign.Models.UserModels;
using DAL.Entities;

namespace ApiDigitalDesign.Services
{
    public class LinkGeneratorService
    {
        public Func<PostAttach, string?>? LinkContentGenerator;
        public Func<User, string?>? LinkAvatarGenerator;
        public void LinkToAvatar(User s, UserAvatarModel d)
        {
            d.AvatarLink = s.Avatar == null ? null : LinkAvatarGenerator?.Invoke(s);
        }
        public void LinkToContent(PostAttach s, AttachLinkModel d)
        {
            d.ContentLink = LinkContentGenerator?.Invoke(s);
        }
    }
}
