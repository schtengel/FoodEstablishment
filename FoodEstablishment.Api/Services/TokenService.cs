using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodEstablishment.Api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FoodEstablishment.Api.Services;

public class TokenService(IConfiguration configuration)
{
    public string GenerateToken(User user)
    {
        var secretKey = configuration["JwtSettings:Secret"] ??
                        throw new InvalidOperationException("JWT Secret is missing.");
        var issuer = configuration["JwtSettings:Issuer"];
        var audience = configuration["JwtSettings:Audience"];
        var expiryMinutes = double.Parse(configuration["JwtSettings:ExpiryInMinutes"] ?? "1440");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            SigningCredentials = creds,
            Issuer = issuer,
            Audience = audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(token);
        
        return tokenHandler.WriteToken(securityToken);
    }
}