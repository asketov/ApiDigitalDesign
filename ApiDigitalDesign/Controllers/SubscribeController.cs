using ApiDigitalDesign.Models.SubscribeModels;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using AutoMapper;
using Common.Exceptions.Subscribe;
using Common.Generics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class SubscribeController : BaseController
    {
        private readonly SubscribeService _subscribeService;
        private readonly IMapper _mapper;
        public SubscribeController(SubscribeService subscribeService, IMapper mapper, LinkGeneratorService links)
        {
            _subscribeService = subscribeService;
            _mapper = mapper;
            links.LinkAvatarGenerator = x =>
                Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
                {
                    userId = x.Id
                });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddSubscribe(AddSubscribeRequest request)
        {
            request.SubscriberId = UserId;
            await _subscribeService.AddSubscribe(_mapper.Map<AddSubscribeModel>(request));
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<List<UserAvatarModel>> GetSubscribers(int skip = 0, int take = 10)
        {
            var models = await _subscribeService.GetSubscribers(skip, take, UserId);
            return models;
        }
        [HttpGet]
        [Authorize]
        public async Task<List<UserAvatarModel>> GetSubscriptions(int skip = 0, int take = 10)
        {
            var models = await _subscribeService.GetSubscriptions(skip, take, UserId);
            return models;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteSubscribe(SubscribeModel request)
        {
            try 
            { 
                await _subscribeService.DeleteSubscribeAsync(request);
                return Ok();
            }
            catch(SubscribeNotFoundException)
            {
                return BadRequest("Подписка не найдена");
            }
        }

    }
}
