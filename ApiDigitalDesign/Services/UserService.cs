using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.UserModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
using Common.Helpers;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Routing;
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
        private Func<Guid, string?>? _linkAvatarGenerator;
        public void SetLinkGenerator(Func<Guid, string?> linkAvatarGenerator)
        {
            _linkAvatarGenerator = linkAvatarGenerator;
        }

        public UserService(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Add avatar to User by Info TempFile and UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>Id created resource.</returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="FileNotExistException"></exception>
        public async Task<Guid> AddAvatarToUser(Guid userId, MetadataModel model)
        {
            var path = AttachService.CopyTempFileToAttaches(model.TempId);
            var user = await _db.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var avatar = new Avatar { Author = user, MimeType = model.MimeType, 
                    FilePath = path, Name = model.Name, Size = model.Size, Id = Guid.NewGuid() };
                _db.Avatars.Add(avatar);
                await _db.SaveChangesAsync();
                return avatar.Id;
            } 
            throw new UserNotFoundException("user with such userId don't exist");
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<AttachModel> GetUserAvatar(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            var attach = _mapper.Map<AttachModel>(user.Avatar);
            return attach;
        }
        /// <summary>
        /// Creating user in context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="UserAlreadyExistException"></exception>
        public async Task<Guid> CreateUserAsync(CreateUserModel model)
        {
            var user = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user!=null) throw new UserAlreadyExistException("such user already exist");
            user = _mapper.Map<User>(model);
            var t =  _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return t.Entity.Id;
        }
        /// <summary>
        /// Get user include avatar by Guid id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _db.Users.Include(x=>x.Avatar).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new UserNotFoundException("user with such id not found");
            return user;
        }
        /// <summary>
        /// Get userModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="UserModel"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<UserAvatarModel> GetUserModelAsync(Guid id)
        {
           var k = _mapper.Map<User, UserAvatarModel>(await GetUserByIdAsync(id));
           return k;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<List<UserAvatarModel>> GetUsersAsync()
        {
            return await _db.Users.AsNoTracking()
                .Include(x => x.Avatar)
                .Include(x => x.Posts)
                .Select(x => _mapper.Map<UserAvatarModel>(x))
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<User> GetUserByCredentionAsync(string email, string password)
        {
            var passwordHash = HashHelper.GetHash(password);
            var user = await _db.Users
                .FirstOrDefaultAsync(user => user.Email == email &
                    passwordHash == user.PasswordHash);
            if (user == null) throw new UserNotFoundException("such user not found");
            return user;
        }
    }
}
