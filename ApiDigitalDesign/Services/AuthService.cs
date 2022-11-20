using System.Security.Claims;
using ApiDigitalDesign.Models.AuthModels;
using Common.Configs;
using Common.Exceptions.Auth;
using Common.Exceptions.User;
using Common.Generics;
using Common.Helpers;
using Common.Statics;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiDigitalDesign.Services
{
    /// <summary>
    /// service for authentication
    /// </summary>
    public class AuthService
    {
        private readonly DataContext _db;
        private readonly AuthConfig _config;
        private readonly SessionService _sessionService;
        private readonly UserService _userService;
        public AuthService(DataContext db, IOptions<AuthConfig> config, SessionService sessionService, UserService userService)
        {
            _db = db;
            _config = config.Value;
            _sessionService = sessionService;
            _userService = userService;
        }
        /// <summary>
        /// Get access+refresh tokens by login+password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Access+refresh tokens - <see cref="TokenModel"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<TokenModel> GetTokensAsync(SignInModel model)
        {
            var user = await _userService.GetUserByCredentionAsync(model.Email, model.Password);
            var session = await _sessionService.CreateSession(user.Id);
            var tokens = GenerateTokens(session);
            if (user.Deleted) await _userService.RecoverAccount(user.Id);
            return tokens;
        }
        /// <summary>
        /// Takes Access+refresh tokens by refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>Access+refresh tokens: <see cref="TokenModel"/></returns>
        /// <exception cref="InvalidTokenException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<TokenModel> GetTokensByRefreshAsync(string refreshToken)
        {
            if (!JwtHelper.ValidateToken(_config, refreshToken)) throw new InvalidTokenException("token is invalid");
            var claims = JwtHelper.GetClaimsFromToken(refreshToken);
            var userId = claims.GetClaimValueOrDefault<Guid>(Auth.UserClaim);
            if (userId == default) throw new InvalidTokenException("user with such id not found");
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user != null)
            {
                var refreshId = claims.GetClaimValueOrDefault<Guid>(Auth.RefreshClaim);
                if (refreshId != default)
                {
                    var session = await  _sessionService.GetSessionByRefreshToken(refreshId);
                    if (!session.IsActive)
                    {
                        throw new InvalidTokenException("session is not active");
                    }
                    session.RefreshTokenId = Guid.NewGuid();
                    await _db.SaveChangesAsync();
                    return GenerateTokens(session);
                }
                throw new InvalidTokenException("refreshTokenId not valid");
            }
            throw new UserNotFoundException("such user don't exist");
        }
        private TokenModel GenerateTokens(UserSession sess)
        {
            var tokens = new TokenModel();
            tokens.AccessToken = JwtHelper.CreateToken(_config, new Claim[]
            {
                new Claim(Auth.UserClaim, sess.UserId.ToString()),
                new Claim(Auth.SessionClaim, sess.Id.ToString())
            }, Auth.AccessTokenLifeTime);
            tokens.RefreshToken = JwtHelper.CreateToken(_config, new Claim[]
            {
                new Claim(Auth.UserClaim, sess.UserId.ToString()),
                new Claim(Auth.RefreshClaim, sess.RefreshTokenId.ToString())
            }, Auth.RefreshTokenLifeTime);
            return tokens;
        }
    }
}
