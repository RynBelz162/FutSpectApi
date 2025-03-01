using FutSpect.Scrapper;
using FutSpect.Scrapper.Interfaces;
using FutSpect.Scrapper.Services.Leagues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScrapperService>();
builder.Services.AddTransient<ILeagueScrapper, MlsLeagueScrapper>();

IHost host = builder.Build();
host.Run();

Console.ReadLine();