using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Configs;
using Microsoft.IdentityModel.Tokens;

namespace Common.Helpers
{
    public static class JwtHelper
    {
        public static string CreateToken(AuthConfig config, Claim[] claims, int lifeTimeInMinutes)
        {
            var dtNow = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                notBefore: dtNow,
                claims: claims,
                expires: DateTime.Now.AddMinutes(lifeTimeInMinutes),
                signingCredentials: new SigningCredentials(config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        public static void ValidateToken(AuthConfig config, string token)
        {
            var validParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = config.SymmetricSecurityKey(),
                ValidAudience = config.Audience,
                ValidIssuer = config.Issuer,
                ClockSkew = TimeSpan.Zero
            };
            new JwtSecurityTokenHandler().ValidateToken(token, validParams, out var securityToken);
        }

        public static Claim[] GetClaimsFromToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.Claims.ToArray();
        }
    }
}
