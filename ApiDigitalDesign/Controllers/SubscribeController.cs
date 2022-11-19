using ApiDigitalDesign.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class SubscribeController : BaseController
    {
        private readonly SubscribeService _subscribeService;
        public SubscribeController(SubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }
    }
}
