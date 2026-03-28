namespace AzuriteBlobDemo.Core.DTOs;

public record FileResponse(Stream Stream, string ContentType);