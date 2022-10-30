using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Common.Configs
{
    public class AuthConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public SymmetricSecurityKey SymmetricSecurityKey()
            => new(Encoding.UTF8.GetBytes(Key));
    }
}
