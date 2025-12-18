namespace Shared.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string name, string email, IEnumerable<string>? roles);
}
