using DAL;
using System.Security.Claims;
using BLL.ModelsDTO.AuthModels;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Common.Configs;
using Common.Exceptions.Auth;
using Common.Exceptions.General;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    /// <summary>
    /// service for authentication
    /// </summary>
    public class AuthService
    {
        private readonly DataContext _db;
        private readonly AuthConfig _config;
        public AuthService(DataContext db, IOptions<AuthConfig> config)
        {
            _db = db;
            _config = config.Value;
        }
        /// <summary>
        /// Get access+refresh tokens by login+password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Access+refresh tokens</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<TokenModelDTO> LoginAsync(SignInDTO dto)
        {
            var passwordHash = HashHelper.GetHash(dto.Password);
            var user = await _db.Users
                .FirstOrDefaultAsync(user => 
                    passwordHash == user.PasswordHash && user.Email == dto.Email);
            if (user != null)
            {
                TokenModelDTO response = new TokenModelDTO();
                response.AccessToken = JwtHelper.CreateToken(_config, new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }, 15);
                response.RefreshToken = JwtHelper.CreateToken(_config, new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }, 1200);
                return response;
            }
            else throw new NotFoundException();
        }
        /// <summary>
        /// Takes Access+refresh tokens by refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>Access+refresh tokens</returns>
        /// <exception cref="InvalidTokenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<TokenModelDTO> RefreshTokenAsync(string refreshToken)
        {
            if (!JwtHelper.ValidateToken(_config, refreshToken)) throw new InvalidTokenException();
            var claims = JwtHelper.GetClaimsFromToken(refreshToken);
            var userId = Guid.Parse(claims.First(x => x.Type == "id").Value);
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user != null)
            {
                TokenModelDTO response = new TokenModelDTO();
                response.AccessToken = JwtHelper.CreateToken(_config, new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }, 15);
                response.RefreshToken = JwtHelper.CreateToken(_config, new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }, 1200);
                return response;
            }
            throw new NotFoundException();
        }
    }
}
