using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Models;

namespace ProjectX.Services;

public class Jwtservice
{
    private readonly IConfiguration _config;

    public Jwtservice(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var secret = jwtSettings["Secret"] ?? "YourSuperSecretKeyHereThatIsAtLeast32BytesLong!";
        var issuer = jwtSettings["Issuer"] ?? "MyWebApi";
        var audience = jwtSettings["Audience"] ?? "MyWebApiClients";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.Username ?? user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role ?? "Candidate"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = issuer,       // 👈 Explicitly writes "iss": "MyWebApi" into payload
            Audience = audience,   // 👈 Explicitly writes "aud": "MyWebApiClients" into payload
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}