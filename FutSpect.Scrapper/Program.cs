using FutSpect.DAL;
using FutSpect.Scrapper;
using FutSpect.Scrapper.Interfaces;
using FutSpect.Scrapper.Services.Leagues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScrapperService>();

const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;";

builder.Services.AddDatabase(ConnectionString);
builder.Services.AddTransient<ILeagueScrapper, MlsLeagueScrapper>();

IHost host = builder.Build();
await host.RunAsync();