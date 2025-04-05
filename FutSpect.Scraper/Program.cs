using FutSpect.DAL;
using FutSpect.Scraper;
using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScraperService>();

const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;";

builder.Services.AddDatabase(ConnectionString);

builder.Services.AddSingleton<IPlayerInfoParseService, PlayerInfoParseService>();

builder.Services.AddSingleton<IMlsLeagueService, MlsLeagueService>();
builder.Services.AddSingleton<ILeagueScraper, MlsLeagueScraper>();

IHost host = builder.Build();
await host.RunAsync();