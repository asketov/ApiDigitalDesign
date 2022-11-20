using ApiDigitalDesign.Models.SubscribeModels;
using AutoMapper;
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
            var recipient = await _userService.GetUserByIdAsync(model.RecipientId);
            if (recipient!.CloseAccount) model.IsAccepted = false;
            var subscribe = _mapper.Map<Subscribe>(model);
            _db.Subscribes.Add(subscribe);
            await _db.SaveChangesAsync();
        }

    }
}
