using Auth.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Common.Services;

namespace Auth.Infrastructure.Jobs;

internal sealed class PasswordReminderHostedService(ILogger logger, IServiceProvider serviceProvider, IConfiguration configuration) 
    : BaseBackgroundService(logger.ForContext<PasswordReminderHostedService>(), null)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Cron-like: run once every day at configured hour (or fallback to every 24 hours).
        var runHour = configuration.GetValue<int?>("AppConfig:ScheduledJobs:PasswordExpiryCheckHourUtc") ?? -1;
        var interval = TimeSpan.FromHours(24);

        Logger.Information("{Name} started.", Name);

        while (!stoppingToken.IsCancellationRequested)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IPasswordReminderService>();

                if (runHour >= 0)
                {
                    // Wait until next runHour UTC
                    var now = DateTime.UtcNow;
                    var next = new DateTime(now.Year, now.Month, now.Day, runHour, 0, 0, DateTimeKind.Utc);
                    if (next <= now) next = next.AddDays(1);
                    var delay = next - now;
                    Logger.Information("{Name} waiting {Delay} until next run at {Next}", Name, delay, next);
                    await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
                    await svc.RunBulkRemindersAsync(stoppingToken).ConfigureAwait(false);
                    // then loop to wait ~24h
                    await Task.Delay(interval, stoppingToken).ConfigureAwait(false);
                }
                else
                {
                    // simple every-24-hours schedule
                    Logger.Information("{Name} running immediate check.", Name);
                    await svc.RunBulkRemindersAsync(stoppingToken).ConfigureAwait(false);
                    await Task.Delay(interval, stoppingToken).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException) { /* shutting down */ }
            catch (Exception ex)
            {
                Logger.Error(ex, "Hosted service {Name} error", Name);
                // On error, wait a short time then retry
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken).ConfigureAwait(false);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        Logger.Information("{Name} stopping.", Name);
    }   
}
