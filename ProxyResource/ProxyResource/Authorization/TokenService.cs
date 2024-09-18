using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;

namespace ProxyResource.Authorization
{
    public class TokenService : ITokenService
    {
        
        private TimeSpan ExpiryDuration = new TimeSpan(0, 5, 0);

        
        public string BuildToken(string key, string issuer, User user)
        {
            
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName), 
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) 
        };

            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            
            var credentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature);

           
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);

            Log.Information("Token created");
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
