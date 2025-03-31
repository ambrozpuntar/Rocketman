using Microsoft.EntityFrameworkCore;
using Rocketer.Data;
using Rocketer.Models.Settings;
using Rocketer.Services.Interfaces;
using Rocketer.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration; // Access configuration through host context

        services.AddHttpClient();

        // Api
        services.Configure<RocketApiSettings>(configuration.GetSection("RocketApi"));
        services.AddScoped<IRocketApiService, RocketApiService>();

        // Email
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailSender, EmailSender>();

        // Database: SQLite
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SqliteConnection")));
        services.AddScoped<ISqliteService, SqliteService>();

        // Worker Service
        services.Configure<RocketWorkerSettings>(configuration.GetSection("RocketWorkerService"));
        services.AddHostedService<RocketWorkerService>(); // Register your worker service
    })
    .Build();

await host.RunAsync();