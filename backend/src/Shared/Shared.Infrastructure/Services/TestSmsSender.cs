using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;

namespace Shared.Infrastructure.Services;

internal sealed class TestSmsSender(ILogger logger/*, IOptions<AppConfig> appConfig*/)
    : BaseService(logger.ForContext<TestSmsSender>(), null), ISmsSender
{
    public async Task SendSmsAsync(string mobileNo, string message, CancellationToken cancellationToken = default)
    {
        //await emailService.SendEmailAsync(mobileNo, "Verify your Mobile Number", message, cancellationToken).ConfigureAwait(false);

        Logger.Debug("Test SMS sent to {MobileNo} with OTP {Otp}", mobileNo, message);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    public async Task SendSmsAsync(string name, string mobileNo, string otp, int expiryMinutes)
    {
        //string body = $@"
        //<p>Hello <strong>{name}</strong>,</p>
        //<p>Welcome to appConfig.Value.AppName!</p>
        //<p>Please use the OTP below to verify your mobile number:</p>
        //<p style='padding:10px 20px; background:#4f46e5; color:white; text-decoration:none; border-radius:6px;'>
        //      {otp}
        //</p>
        //<p>This otp will expire in {expiryMinutes} Minutes.</p>
        //";

        //await emailService.SendEmailAsync(mobileNo, "Verify your Mobile Number", body).ConfigureAwait(false);

        Logger.Debug("Test SMS sent to {MobileNo} with OTP {Otp}", mobileNo, otp);
    }
}
