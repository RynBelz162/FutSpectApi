using FutSpect.Dal;
using FutSpect.Scraper.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Configuration
    .AddJsonFile("./FutSpect.Scraper/appsettings.json", optional: false, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("FutSpect")
    ?? throw new InvalidOperationException("Connection string 'FutSpect' not found.");

builder.Services
    .AddHttpClient()
    .AddConfigOptions(builder.Configuration, args)
    .AddSingleton(TimeProvider.System)
    .AddDatabase(connectionString)
    .AddServices()
    .AddScrapers()
    .AddBackgroundJobs();

IHost host = builder.Build();
await host.RunAsync();