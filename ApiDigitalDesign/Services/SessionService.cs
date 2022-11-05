using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Common.Exceptions.Auth;
using Common.Exceptions.General;

namespace ApiDigitalDesign.Services
{
    /// <summary>
    /// service for manage sessions
    /// </summary>
    public class SessionService
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public SessionService(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Create session by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>session</returns>
        public async Task<UserSession> CreateSession(Guid userId)
        {
            var userSession = new UserSession()
            {
                Created = DateTimeOffset.UtcNow,
                IsActive = true,
                RefreshTokenId = Guid.NewGuid(),
                UserId = userId
            };
            var t = _db.UserSessions.Add(userSession);
            await _db.SaveChangesAsync();
            return t.Entity;
        }
        /// <summary>
        /// get session by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserSession</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserSession> GetSessionById(Guid id)
        {
            var session = await _db.UserSessions.FirstOrDefaultAsync(x => x.Id == id);
            if (session == null)
            {
                throw new NotFoundException();
            }
            return session;
        }
        /// <summary>
        /// get session by his refreshTokenId
        /// </summary>
        /// <param name="refreshTokenId"></param>
        /// <returns>userSession</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserSession> GetSessionByRefreshToken(Guid refreshTokenId)
        {
            var sess = await _db.UserSessions.Include(us => us.User)
                .FirstOrDefaultAsync(us => us.RefreshTokenId == refreshTokenId);
            if (sess == null)
            {
                throw new InvalidTokenException();
            }
            return sess;
        }




    }
}
