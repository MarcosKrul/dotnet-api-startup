using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TucaAPI.Common;
using TucaAPI.Providers;
using TucaAPI.Models;
using TucaAPI.src.Common;

namespace TucaAPI.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;
        private readonly UserManager<AppUser> userManager;

        public TokenProvider(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            var secretKey = this.configuration[EnvVariables.JWT_SIGNIN_KEY] ?? Constants.DEFAULT_JWT_SECRET;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        private async Task<List<Claim>> GetClaimsAsync(AppUser user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
                throw new Exception(MessageKey.REQUIRED_USER_INFOS);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.UserName),
            };

            var roles = await this.userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            return claims;
        }

        private string GetFormattedToken(SecurityTokenDescriptor tokenDescription)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> CreateAsync(AppUser user)
        {
            var credentials = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(await this.GetClaimsAsync(user)),
                Expires = DateTime.Now.AddDays(Constants.JWT_DAYS_TO_EXPIRES),
                SigningCredentials = credentials,
                Issuer = this.configuration[EnvVariables.JWT_ISSUER],
                Audience = this.configuration[EnvVariables.JWT_AUDIENCE]
            };

            return this.GetFormattedToken(tokenDescription);
        }
    }
}