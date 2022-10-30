using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.ModelsDTO.AuthModels;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Common.Configs;
using Common.Exceptions;
using Common.Exceptions.General;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly DataContext _db;
        private readonly AuthConfig _config;
        public AuthService(DataContext db, IOptions<AuthConfig> config)
        {
            _db = db;
            _config = config.Value;
        }
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

        public async Task<TokenModelDTO> RefreshToken(string refreshToken)
        {
            JwtHelper.ValidateToken(_config, refreshToken);
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
