using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusTrackBackEnd.API.IAM.Infrastructure.Tokens.JWT.Services;

public class TokenService : ITokenService
{
    private readonly TokenSettings _settings;

    public TokenService(IOptions<TokenSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}