using ApiDigitalDesign.Models.UserModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions.General;
using Common.Helpers;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiDigitalDesign.Services
{
    /// <summary>
    /// Service for manage users
    /// </summary>
    public class UserService
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public UserService(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Creating user in context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="AlreadyExistException"></exception>
        public async Task<Guid> CreateUserAsync(CreateUserModel model)
        {
            var user = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user!=null) throw new AlreadyExistException();
            user = _mapper.Map<User>(model);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }
        /// <summary>
        /// Get user by Guid id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<GetUserModel> GetUserAsync(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new NotFoundException();
            return _mapper.Map<GetUserModel>(user);
        }
        /// <summary>
        /// Get user by Guid id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<List<GetUserModel>> GetUsersAsync()
        {
            var users = await _db.Users.AsNoTracking()
                .ProjectTo<GetUserModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (users == null)
                throw new NotFoundException();
            return users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<User> GetUserByCredention(string email, string password)
        {
            var passwordHash = HashHelper.GetHash(password);
            var user = await _db.Users
                .FirstOrDefaultAsync(user => user.Email == email &
                    passwordHash == user.PasswordHash);
            if (user == null) throw new NotFoundException();
            return user;
        }
    }
}
