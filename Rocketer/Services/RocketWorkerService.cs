
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rocketer.Data;
using Rocketer.Mappings.LaunchMappers;
using Rocketer.Models.Settings;
using Rocketer.Services.Interfaces;

namespace Rocketer.Services;

public class RocketWorkerService : IHostedService
{
    private readonly RocketWorkerSettings _settings;
    private readonly ILogger<RocketWorkerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly List<string> _recipents;
    private bool _stopping = false;

    public RocketWorkerService(
        IOptions<RocketWorkerSettings> settings,
        ILogger<RocketWorkerService> logger,
        IServiceProvider serviceProvider,
        IOptions<EmailSettings> emailSettings
    )
    {
        _settings = settings.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _recipents = emailSettings.Value.Recipents;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_settings.SyncEnabled)
            await StopAsync(cancellationToken).ConfigureAwait(false);

        _stopping = false;

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // Apply pending migrations
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        

        await RunWorker(cancellationToken);
    }

    public async Task RunWorker(CancellationToken ct)
    {
        var interval = TimeSpan.FromDays(_settings.IntervalDays);

        while (!_stopping)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var rocketApiService = scope.ServiceProvider.GetRequiredService<IRocketApiService>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var sqliteService = scope.ServiceProvider.GetRequiredService<ISqliteService>();

                if (ct.IsCancellationRequested)
                {
                    _stopping = true;
                    ct.ThrowIfCancellationRequested();
                }

                //Get launches
                var launches = await rocketApiService.GetLaunchesAsync();

                foreach (var launch in launches)
                {
                    if (string.IsNullOrWhiteSpace(launch.Id))
                        continue;
                    var result = await sqliteService.GetLaunchByIdAsync(launch.Id);

                    //If launch exists and statuses are the same, skip
                    if (result is not null && result.Status == launch.Status)
                    {
                        continue;
                    }
                    else if (result is not null && result.Status != launch.Status)
                    {
                        result.NotifiedStatus = Enums.NotifiedStatus.NotNotified;
                        result.LaunchDate = launch.LaunchDate;
                        result.Status = launch.Status;

                        var isUpdated = await sqliteService.UpdateLaunchAsync(result);
                    }

                    //If launch doesnt exist, add to database
                    if (result is null)
                    {
                        var toLaunch = launch.ToLaunch();
                        toLaunch.NotifiedStatus = Enums.NotifiedStatus.NotNotified;
                        await sqliteService.AddLaunchAsync(toLaunch);
                        continue;
                    }
                }

                var unotifiedLaunches = await sqliteService.GetUnnotifiedLaunchesAsync();
                if (unotifiedLaunches.Count > 0)
                {
                    var emailBody = new StringBuilder();
                    emailBody.AppendLine("<html><body>");
                    foreach (var launch in unotifiedLaunches)
                    {
                        emailBody.AppendLine($"<p>Launch: {launch.Id} - {launch.LaunchDate} - {launch.Status}<p></br>");
                    }

                    emailBody.AppendLine("</body></html>");

                    foreach (var recipent in _recipents)
                    {
                        await emailSender.SendEmailAsync(recipent, "Rocket Launches", emailBody.ToString());
                    }

                    foreach (var launch in unotifiedLaunches)
                    {
                        launch.NotifiedStatus = Enums.NotifiedStatus.Notified;
                        await sqliteService.UpdateLaunchAsync(launch);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            await Task.Delay(interval, ct).ConfigureAwait(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rocket Worker Service is stopping.");
        return Task.CompletedTask;
    }
}