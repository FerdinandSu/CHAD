using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Chad.Services
{
    public class JwtManager
    {
        public JwtManager(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
            JwtSettings = Configuration.GetSection("JwtSettings");
        }

        private IConfiguration Configuration { get; }
        private IConfigurationSection JwtSettings { get; }

        public static ValueTask<Claim[]> GetLoginClaimsAsync(DbUser user)
        {
            return new(new[]
            {
                new Claim(ClaimTypes.Name, user.FriendlyName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                AccountManager.GetRoleClaim(user.Role)
            });
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, Claim[] claims)
        {
            return new(
                JwtSettings.GetSection("validIssuer").Value,
                JwtSettings.GetSection("validAudience").Value,
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: signingCredentials);
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(JwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}