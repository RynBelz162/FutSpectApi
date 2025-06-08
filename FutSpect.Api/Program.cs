using FutSpect.Api.Extensions;
using FutSpect.Dal;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new()
        {
            Title = "FutSpect API",
            Version = "v1",
            Description = "Modern API for all football information.",
        };
        return Task.CompletedTask;
    });
});

var connectionString = builder.Configuration.GetConnectionString("FutSpect")
    ?? throw new InvalidOperationException("Connection string 'FutSpect' not found.");

builder.Services.AddControllers();
builder.Services
    .AddDatabase(connectionString)
    .AddServices();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "FutSpect API";
        opt.Favicon = "/favicon.ico";
        opt.Theme = ScalarTheme.BluePlanet;
    });
}

app.MapGet("/", () => "Welcome to FutSpect API!")
    .ExcludeFromDescription();

app.UseHttpsRedirection();

app.ServeFavicon();

app.MapControllers();

app.Run();