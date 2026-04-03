using AzuriteBlobDemo.API.Endpoints;
using AzuriteBlobDemo.API.Extensions;
using AzuriteBlobDemo.Infrastructure.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptions<CloudInfoOptions>()
    .BindConfiguration(CloudInfoOptions.SectionName)
    .ValidateOnStart();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Azurite Blob API");
        options.DisableAgent();
    });
}

app.UseHttpsRedirection();

app.MapFilesRoute();

app.Run();