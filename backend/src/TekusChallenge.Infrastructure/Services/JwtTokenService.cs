using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secret = _configuration["Config:Secret"]
            ?? throw new InvalidOperationException("JWT Secret is not configured");
        var key = Encoding.ASCII.GetBytes(secret);

        var expirationMinutes = int.Parse(_configuration["Config:ExpirationMinutes"] ?? "60");
        var expirationTime = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("username", username)
            }),
            Expires = expirationTime,
            Issuer = _configuration["Config:Issuer"],
            Audience = _configuration["Config:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public DateTime GetTokenExpiration()
    {
        var expirationMinutes = int.Parse(_configuration["Config:ExpirationMinutes"] ?? "60");
        return DateTime.UtcNow.AddMinutes(expirationMinutes);
    }
}
