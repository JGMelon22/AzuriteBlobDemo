using Azure;
using Azure.Storage.Blobs.Models;
using AzuriteBlobDemo.Core.DTOs;

namespace AzuriteBlobDemo.Infrastructure.Interfaces.Services;

public interface IBlobService
{
    Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default);
    IAsyncEnumerable<BlobItemResponse> ListAllAsync();
    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
}