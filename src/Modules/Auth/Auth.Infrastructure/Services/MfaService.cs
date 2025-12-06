using Auth.Application.Interfaces;
using Serilog;
using Shared.Common.Services;

namespace Auth.Infrastructure.Services;

public sealed class MfaService(ILogger logger) : BaseService(logger.ForContext<MfaService>(), null), IMfaService
    {
        public string GenerateSecret() => Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
        public string GetProvisionUrl(string secret, string issuer, string account)
        {
            var label = Uri.EscapeDataString($"{issuer}:{account}");
            return $"otpauth://totp/{label}?secret={secret}&issuer={Uri.EscapeDataString(issuer)}&digits=6";
        }
        public bool ValidateTotp(string secret, string code)
        {
            var bytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(bytes);
            return totp.VerifyTotp(code, out long _, new VerificationWindow(1, 1));
        }
    }
