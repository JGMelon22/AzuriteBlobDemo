using Azure.Storage.Blobs;
using AzuriteBlobDemo.Infrastructure.Interfaces.Services;
using AzuriteBlobDemo.Infrastructure.Services;

namespace AzuriteBlobDemo.API.Extensions;

public static class IocExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));
    }
}