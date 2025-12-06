using System.Security.Cryptography;

namespace Shared.Security;

// PBKDF2 implementation suitable for production. Salt + iterations encoded in output.
internal sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public string Hash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var key = deriveBytes.GetBytes(KeySize);

        var parts = new byte[1 + SaltSize + KeySize];
        parts[0] = 0; // version
        Buffer.BlockCopy(salt, 0, parts, 1, SaltSize);
        Buffer.BlockCopy(key, 0, parts, 1 + SaltSize, KeySize);
        return Convert.ToBase64String(parts);
    }

    public bool Verify(string hashed, string password)
    {
        var parts = Convert.FromBase64String(hashed);
        if (parts.Length != 1 + SaltSize + KeySize) return false;
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(parts, 1, salt, 0, SaltSize);
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var key = deriveBytes.GetBytes(KeySize);
        var storedKey = new byte[KeySize];
        Buffer.BlockCopy(parts, 1 + SaltSize, storedKey, 0, KeySize);
        return CryptographicOperations.FixedTimeEquals(key, storedKey);
    }
}
