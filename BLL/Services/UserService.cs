using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ModelsDTO.UserModels;
using DAL;

namespace BLL.Services
{
    public class UserService
    {
        private readonly DataContext _db;

        public UserService(DataContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateUserAsync(CreateUserDTO model)
        {
            var user = model.DtoToUser();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }
    }
}
