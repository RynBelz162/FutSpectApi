using FutSpect.DAL;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("FutSpect")
    ?? throw new InvalidOperationException("Connection string 'FutSpect' not found.");

builder.Services.AddControllers();
builder.Services.AddDatabase(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Welcome to FutSpect API!");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();