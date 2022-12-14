using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.AuthModels;
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
        public async Task AddAvatarToUserAsync(Guid userId, MetadataModel model)
        {
            var path = AttachService.CopyTempFileToAttaches(model.TempId);
            var user = await GetUserByIdAsync(userId);
            user.Avatar = new Avatar { Author = user, MimeType = model.MimeType, 
                FilePath = path, Name = model.Name, Size = model.Size };
            await _db.SaveChangesAsync();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="AvatarNotFoundException"></exception>
        public async Task<AttachModel> GetUserAvatarAsync(Guid userId)
        {
            var avatar = await _db.Avatars.FirstOrDefaultAsync(u => u.UserId == userId);
            if (avatar == null) throw new AvatarNotFoundException("avatar not exist");
            var attach = _mapper.Map<AttachModel>(avatar);
            return attach;
        }
        /// <summary>
        /// Creating user in context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="UserAlreadyExistException"></exception>
        public async Task CreateUserAsync(CreateUserModel model)
        {
            var user = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null) throw new UserAlreadyExistException("such user already exist");
            user = _mapper.Map<User>(model);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
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

        public async Task ChangeVisibilityAccount(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            user.CloseAccount = !user.CloseAccount;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAccount(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            user.Deleted = true;
            await _db.SaveChangesAsync();
        }
        public async Task RecoverAccount(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            user.Deleted = false;
            await _db.SaveChangesAsync();
        }
    }
}
