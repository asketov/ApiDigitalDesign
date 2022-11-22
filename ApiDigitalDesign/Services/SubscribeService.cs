using ApiDigitalDesign.Controllers;
using ApiDigitalDesign.Models.SubscribeModels;
using ApiDigitalDesign.Models.UserModels;
using AutoMapper;
using Common.Exceptions.Subscribe;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiDigitalDesign.Services
{
    public class SubscribeService
    {
        private readonly DataContext _db;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        public SubscribeService(DataContext db, IMapper mapper, UserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task AddSubscribe(AddSubscribeModel model)
        {
            var sub = await GetSubscribeAsync(_mapper.Map<SubscribeModel>(model));
            if (sub != null) throw new SubscribeAlreadyExistException();
            var recipient = await _userService.GetUserByIdAsync(model.RecipientId);
            if (recipient!.CloseAccount) model.IsAccepted = false;
            var subscribe = _mapper.Map<Subscribe>(model);
            _db.Subscribes.Add(subscribe);
            await _db.SaveChangesAsync();
        }
        public async Task<Subscribe?> GetSubscribeAsync(SubscribeModel model)
        {
            var sub = await _db.Subscribes.FirstOrDefaultAsync(u =>
                u.SubscriberId == model.SubscriberId && u.RecipientId == model.RecipientId);
            return sub;
        }

        public async Task<List<UserAvatarModel>> GetSubscribers(int skip, int take, Guid userId)
        {
            var subscribers = await _db.Subscribes.Where(u => u.IsAccepted && u.RecipientId == userId).Include(f=>f.Subscriber.Avatar)
                .AsNoTracking().OrderByDescending(x=>x.Created).Skip(skip).Take(take)
                .Select(x =>_mapper.Map<UserAvatarModel>(x.Subscriber)).ToListAsync();
            return subscribers;
        }
        public async Task<List<UserAvatarModel>> GetSubscriptions(int skip, int take, Guid userId)
        {
            var subscribers = await _db.Subscribes.Where(u => u.IsAccepted && u.SubscriberId == userId).Include(f => f.Subscriber.Avatar)
                .AsNoTracking().OrderByDescending(x => x.Created).Skip(skip).Take(take)
                .Select(x => _mapper.Map<UserAvatarModel>(x.Subscriber)).ToListAsync();
            return subscribers;
        }
        public async Task DeleteSubscribeAsync(SubscribeModel model)
        {
            var sub = await GetSubscribeAsync(model);
            if (sub == null) throw new SubscribeNotFoundException();
            _db.Subscribes.Remove(sub);
            await _db.SaveChangesAsync();
        }

    }
}
