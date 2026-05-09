using AzuriteBlobDemo.API.Endpoints;
using AzuriteBlobDemo.API.Extensions;
using AzuriteBlobDemo.Infrastructure.Options;
using Microsoft.Net.Http.Headers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Enable Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .WithHeaders(HeaderNames.ContentType, "multipart/form-data")
            .WithMethods("POST", "GET", "DELETE");
        });
});

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

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.MapFilesRoute();

app.Run();