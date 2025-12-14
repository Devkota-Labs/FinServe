namespace Shared.Application.Interfaces;

public interface IOtpGenerator
{
    string GenerateNumericOtp(int length = 6);
    string GenerateAlphaNumericOtp(int length = 6);
    string GenerateSecureToken(int length = 32);
}
