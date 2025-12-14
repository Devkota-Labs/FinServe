namespace Auth.Application.Interfaces.Services;

public interface IMfaService
{
    string GenerateSecret();
    Uri GetProvisionUrl(string secret, string issuer, string account);
    bool ValidateTotp(string secret, string code);
}
