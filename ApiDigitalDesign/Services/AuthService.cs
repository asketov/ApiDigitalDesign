using System.Security.Claims;
using ApiDigitalDesign.Models.AuthModels;
using Common.Configs;
using Common.Exceptions.Auth;
using Common.Exceptions.User;
using Common.Helpers;
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
            if (!JwtHelper.ValidateToken(_config, refreshToken)) throw new InvalidTokenException();
            var claims = JwtHelper.GetClaimsFromToken(refreshToken);
            var userId = Guid.Parse(claims.First(x => x.Type == "userId").Value);
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user != null)
            {
                if (claims.FirstOrDefault(x => x.Type == "refreshTokenId")?.Value is String refreshIdString
                    && Guid.TryParse(refreshIdString, out var refreshId))
                {
                    var session = await  _sessionService.GetSessionByRefreshToken(refreshId);
                    if (!session.IsActive)
                    {
                        throw new InvalidTokenException();
                    }
                    session.RefreshTokenId = Guid.NewGuid();
                    await _db.SaveChangesAsync();
                    return GenerateTokens(session);
                }
                throw new InvalidTokenException();
            }
            throw new UserNotFoundException();
        }
        private TokenModel GenerateTokens(UserSession sess)
        {
            var tokens = new TokenModel();
            tokens.AccessToken = JwtHelper.CreateToken(_config, new Claim[]
            {
                new Claim("userId", sess.UserId.ToString()),
                new Claim("sessionId", sess.Id.ToString())
            }, 15);
            tokens.RefreshToken = JwtHelper.CreateToken(_config, new Claim[]
            {
                new Claim("userId", sess.UserId.ToString()),
                new Claim("refreshTokenId", sess.RefreshTokenId.ToString())
            }, 1200);
            return tokens;
        }
    }
}
