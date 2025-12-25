using Microsoft.IdentityModel.Tokens;
using Shared.Security.Configurations;
using System.Security.Claims;
using System.Text;

namespace Shared.Security;

public static class TokenValidationParametersFactory
{
    public static TokenValidationParameters Create(JwtOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var secret = options.Key ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,                
            RoleClaimType = ClaimTypes.Role,
        };
    }
}
