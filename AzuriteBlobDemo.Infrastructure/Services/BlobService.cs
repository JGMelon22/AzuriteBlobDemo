using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzuriteBlobDemo.Core.DTOs;
using AzuriteBlobDemo.Core.Exceptions;
using AzuriteBlobDemo.Infrastructure.Interfaces.Services;
using AzuriteBlobDemo.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace AzuriteBlobDemo.Infrastructure.Services;

public class BlobService(
    BlobServiceClient blobServiceClient,
    IOptions<CloudInfoOptions> cloudInfoOptions)
    : IBlobService
{
    private readonly string _containerName = cloudInfoOptions.Value.ContainerName!;

    public async Task<Guid> UploadAsync(Stream stream, string contentType,
        CancellationToken cancellationToken = default)
    {
        const int maxAllowedFileSize = 10 * 1024 * 1024; // 10 MB
        if (stream.Length >= maxAllowedFileSize)
            throw new FileToLargeException($"File size can not exceed {maxAllowedFileSize} MB");
        
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var fileId = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken
        );

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        Response<BlobDownloadResult> response =
            await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(response.Value.Content.ToStream(),
            response.Value.Details.ContentType);
    }

    public async IAsyncEnumerable<BlobItemResponse> ListAllAsync()
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            yield return new BlobItemResponse(
                Name: blobItem.Name,
                ContentType: blobItem.Properties.ContentType);
        }
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}