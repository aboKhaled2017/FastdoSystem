using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Services
{
    public class JWThandlerService
    {
        public readonly IConfiguration _configuration;
        private readonly IConfigurationSection _JWT = RequestStaticServices.GetConfiguration.GetSection("JWT");
        public JWThandlerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenModel CreateAccessToken(AppUser user, string role)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim("UserId",user.Id),
                new Claim("Email",user.Email),
                new Claim("Phone",user.PhoneNumber),
                new Claim("UserName",user.UserName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.GetValue<string>("signingKey")));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //addClaims(claims);
            var jwt = CreateSecurityToken(claims, DateTime.UtcNow.AddYears(10), signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return CreateTokenModel(token, DateTime.UtcNow.Ticks);
        }
        public TokenModel CreateRefreshToken(AppUser user)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.GetValue<string>("signingKey")));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //addClaims(claims);
            var jwt = CreateSecurityToken(claims, DateTime.UtcNow.AddYears(5), signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return CreateTokenModel(token, DateTime.UtcNow.Ticks);
        }
        private JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims, DateTime expiry, SigningCredentials signingCredentials)
            => new JwtSecurityToken(
                issuer: _JWT.GetValue<string>("issuer"),
                expires: expiry,
                audience: _JWT.GetValue<string>("audience"),
                claims: claims,
                signingCredentials: signingCredentials
                );
        private static TokenModel CreateTokenModel(string token, long expiry)
            => new TokenModel { token = token, expiry = expiry };
        public AuthTokensModel GetAuthTokensModel(AppUser user, string role)
        {
            return new AuthTokensModel
            {
                accessToken = CreateAccessToken(user, role),
                refreshToken = CreateRefreshToken(user)
            };
        }
    }
}
