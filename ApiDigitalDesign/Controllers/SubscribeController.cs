using ApiDigitalDesign.Models.SubscribeModels;
using ApiDigitalDesign.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;
        public SubscribeController(SubscribeService subscribeService, IMapper mapper)
        {
            _subscribeService = subscribeService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task AddSubscribe(AddSubscribeRequest request)
        {
            request.SubscriberId = UserId;
            await _subscribeService.AddSubscribe(_mapper.Map<AddSubscribeModel>(request));
        }
    }
}
