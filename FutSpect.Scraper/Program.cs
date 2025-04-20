using FutSpect.DAL;
using FutSpect.Scraper.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("FutSpect")
    ?? throw new InvalidOperationException("Connection string 'FutSpect' not found.");

builder.Services
    .AddDatabase(connectionString)
    .AddServices()
    .AddScrapers();

IHost host = builder.Build();
await host.RunAsync();