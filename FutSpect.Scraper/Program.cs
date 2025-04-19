using FutSpect.DAL;
using FutSpect.Scraper;
using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Scrapers.Mls;
using FutSpect.Scraper.Scrapers.Usl;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Scraper.Services.Leagues.Usl;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScraperService>();

const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;";

builder.Services.AddDatabase(ConnectionString);

builder.Services.AddSingleton<IPlayerInfoParseService, PlayerInfoParseService>();
builder.Services.AddSingleton<ISanitizeService, SanitizeService>();

builder.Services.AddSingleton<IMlsLeagueService, MlsLeagueService>();
builder.Services.AddSingleton<IUslLeagueService, UslLeagueService>();
builder.Services.AddSingleton<ILeagueScraper, MlsLeagueScraper>();
builder.Services.AddSingleton<ILeagueScraper, UslLeagueScraper>();

IHost host = builder.Build();
await host.RunAsync();