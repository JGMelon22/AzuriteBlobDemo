using AzuriteBlobDemo.Core.DTOs;
using AzuriteBlobDemo.Infrastructure.Interfaces.Services;

namespace AzuriteBlobDemo.API.Endpoints;

public static class FilesEndpoint
{
    public static void MapFilesRoute(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("files", async (IFormFile file, IBlobService blobService) =>
            {
                using Stream stream = file.OpenReadStream();

                Guid fileId = await blobService.UploadAsync(stream, file.ContentType);

                return Results.Ok(fileId);
            })
            .WithTags("Files")
            .DisableAntiforgery();

        endpoints.MapDelete("files/{fileId}", async (Guid fileId, IBlobService blobService) =>
            {
                await blobService.DeleteAsync(fileId);

                return Results.NoContent();
            })
            .WithTags("Files");

        endpoints.MapGet("files/{fileId}", async (Guid fileId, IBlobService blobService) =>
            {
                FileResponse fileResponse = await blobService.DownloadAsync(fileId);

                return Results.File(fileResponse.Stream, fileResponse.ContentType);
            })
            .WithTags("Files");

        endpoints.MapGet("files/", async (IBlobService blobService) =>
            {
                List<BlobItemResponse> blobs = await blobService.ListAllAsync().ToListAsync();

                return Results.Ok(blobs);
            })
            .WithTags("Files");
    }
}