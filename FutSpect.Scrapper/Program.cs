using FutSpect.Scrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ScrapperService>();

IHost host = builder.Build();
host.Run();

Console.ReadLine();