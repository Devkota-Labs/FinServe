using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared.Security.Configurations;
using Microsoft.Extensions.Options;
using Shared.Common.Services;
using Serilog;
using Shared.Common;

namespace Shared.Security;

internal sealed class JwtTokenGenerator(ILogger logger, IOptions<JwtOptions> jwtOptions) 
    : BaseService(logger.ForContext<JwtTokenGenerator>(), jwtOptions.Value), IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateToken(int userId, string name, string email, IEnumerable<string> roles)
    {
        var secret = _jwtOptions.Key ?? throw new InvalidOperationException("AppConfig:Jwt:Key not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString(Constants.IFormatProvider)),
            new("UserId", userId.ToString(Constants.IFormatProvider)),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.NameIdentifier, name),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
