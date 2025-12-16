using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Shared.Security;

public static class TokenValidationParametersFactory
{
    public static TokenValidationParameters Create(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var secret = config["AppConfig:Jwt:Key"] ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,                
            ValidIssuer = config["AppConfig:Jwt:Issuer"],
            ValidAudience = config["AppConfig:Jwt:Audience"],
            RoleClaimType = ClaimTypes.Role,
        };
    }
}
