using Common.Generics;
using Common.Statics;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid UserId  => User.Claims.GetClaimValueOrDefault<Guid>(Auth.UserClaim);

    }
}
