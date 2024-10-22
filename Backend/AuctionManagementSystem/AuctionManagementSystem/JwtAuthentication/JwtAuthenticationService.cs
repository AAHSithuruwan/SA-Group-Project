using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionManagementSystem.JwtAuthentication
{
    public class JwtAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public JwtAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId)
        {
            var jwtSecretKey = _configuration["Jwt:SecretKey"];

            if (jwtSecretKey == null)
            {
                throw new Exception("JWT Secret Key is null");
            }

            var jwtIssuer = _configuration["Jwt:Issuer"];

            if (jwtIssuer == null)
            {
                throw new Exception("JWT Issuer is null");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
