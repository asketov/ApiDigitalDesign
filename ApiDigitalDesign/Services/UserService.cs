using ApiDigitalDesign.Models.UserModels;
using Common.Exceptions.General;
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

        public UserService(DataContext db)
        {
            _db = db;
        }
        /// <summary>
        /// Creating user in context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="AlreadyExistException"></exception>
        public async Task<Guid> CreateUserAsync(CreateUserModel model)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user!=null) throw new AlreadyExistException();
            user = model.DtoToUser();
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
        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new NotFoundException();
            return user;
        }
    }
}
