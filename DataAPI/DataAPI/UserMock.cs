using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAPI
{
    public class UserMock
    {
        
        public UserMock()
        {
           
        }
        public static string AdminToken { get; private set; }
        public static string UserToken { get; private set; }

        public static void InitializeTokens(IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var key = jwtSettings["Key"];

            AdminToken = GenerateToken("admin", "admin", issuer, audience, key);
            UserToken = GenerateToken("user", "user", issuer, audience, key);
        }

        private static string GenerateToken(string username, string role, string issuer, string audience, string secretKey)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                expires: DateTime.UtcNow.AddYears(100),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

