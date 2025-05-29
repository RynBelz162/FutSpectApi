using FutSpect.Api.Extensions;
using FutSpect.DAL;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new()
        {
            Title = "FutSpect API",
            Version = "v1",
            Description = """
                Modern API for football information.
                Supports JSON responses.
                """,
            Contact = new()
            {
                Name = "API Support",
                Email = "support@futspect.com",
                Url = new Uri("https://futspect.com/support")
            }
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "FutSpect API";
        opt.Theme = ScalarTheme.BluePlanet;
    });
}

app.MapGet("/", () => "Welcome to FutSpect API!")
    .ExcludeFromDescription();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();