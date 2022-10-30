using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelsDTO.AuthModels
{
    public class TokenModelDTO
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
