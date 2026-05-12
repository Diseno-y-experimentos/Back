using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
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
        return GenerateToken(CreateTokenClaims(user.Id, user.Username, "user"));
    }

    public string GenerateToken(Company company)
    {
        return GenerateToken(CreateTokenClaims(company.Id, company.Email, "company"));
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
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

    private static IEnumerable<Claim> CreateTokenClaims(int id, string username, string accountType)
    {
        return new[]
        {
            new Claim("id", id.ToString()),
            new Claim("username", username),
            new Claim("account_type", accountType)
        };
    }
}