using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Fastdo.Core.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Services
{
    public class JWThandlerService
    {
        public readonly IConfiguration _configuration;
        private readonly IConfigurationSection _JWT = RequestStaticServices.GetConfiguration().GetSection("JWT");
        public JWThandlerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenModel CreateAccessToken_ForAdministartor(AppUser user,string name,IList<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(Variables.UserClaimsTypes.UserId,user.Id),
                new Claim(ClaimTypes.Name,name),
                new Claim(Variables.UserClaimsTypes.Phone,user.PhoneNumber),
                new Claim(Variables.UserClaimsTypes.UserName,user.UserName),
                new Claim(ClaimTypes.Role,Variables.adminer),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString())
            };
            claims.AddRange(userClaims);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.GetValue<string>("signingKey")));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //addClaims(claims);
            var jwt = CreateSecurityToken(claims, DateTime.UtcNow.AddYears(10), signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return CreateTokenModel(token, DateTime.UtcNow.Ticks);
        }
        public TokenModel CreateAccessToken(AppUser user, string role,string compName,Action<List<Claim>> addOtherClaims=null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim("UserId",user.Id),
                new Claim("Email",user.Email),
                new Claim(ClaimTypes.Name,compName),
                new Claim("Phone",user.PhoneNumber),
                new Claim("IsEmailConfirmed",user.EmailConfirmed.ToString()),
                new Claim("UserName",user.UserName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            };
            addOtherClaims?.Invoke(claims);
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
        public AuthTokensModel GetAuthTokensModel(AppUser user, string role,string compName)
        {
            return new AuthTokensModel
            {
                accessToken = CreateAccessToken(user, role,compName),
                refreshToken = CreateRefreshToken(user)
            };
        }
    }
}
