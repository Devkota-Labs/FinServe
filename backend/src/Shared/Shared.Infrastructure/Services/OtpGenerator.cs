using Serilog;
using Shared.Application.Interfaces;
using Shared.Common.Services;
using System.Security.Cryptography;
using System.Text;

internal sealed class OtpGenerator(ILogger logger) : BaseService(logger.ForContext<OtpGenerator>(), null), IOtpGenerator
{
    public string GenerateNumericOtp(int length = 6)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        var otp = new StringBuilder(length);

        foreach (var b in bytes)
            otp.Append((b % 10).ToString());

        return otp.ToString();
    }

    public string GenerateAlphaNumericOtp(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = RandomNumberGenerator.GetBytes(length);

        var result = new StringBuilder(length);
        foreach (var b in bytes)
            result.Append(chars[b % chars.Length]);

        return result.ToString();
    }

    public string GenerateSecureToken(int length = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes)
            .Replace("/", "_")
            .Replace("+", "-")
            .Substring(0, length);
    }
}