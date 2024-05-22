using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TucaAPI.Common;
using TucaAPI.Interfaces;
using TucaAPI.Models;

namespace TucaAPI.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
            var secretKey = this.configuration["JWT:SigningKey"] ?? Constants.DEFAULT_JWT_SECRET;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        private List<Claim> GetClaims(AppUser user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
                throw new Exception("Required user infos");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.UserName),
            };

            return claims;
        }

        private string GetFormattedToken(SecurityTokenDescriptor tokenDescription)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public string Create(AppUser user)
        {
            var credentials = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(this.GetClaims(user)),
                Expires = DateTime.Now.AddDays(Constants.JWT_DAYS_TO_EXPIRES),
                SigningCredentials = credentials,
                Issuer = this.configuration["JWT:Issuer"],
                Audience = this.configuration["JWT:Audience"]
            };

            return this.GetFormattedToken(tokenDescription);
        }
    }
}