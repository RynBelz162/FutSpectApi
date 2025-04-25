using FutSpect.DAL;
using FutSpect.Scraper.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Configuration
    .AddJsonFile("./FutSpect.Scraper/appsettings.json", optional: false, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("FutSpect")
    ?? throw new InvalidOperationException("Connection string 'FutSpect' not found.");

builder.Services
    .AddDatabase(connectionString)
    .AddServices()
    .AddScrapers()
    .AddBackgroundJobs();

IHost host = builder.Build();
await host.RunAsync();