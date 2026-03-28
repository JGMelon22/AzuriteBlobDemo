# AzuriteBlobDemo

A .NET 10 Web API demonstrating Azure Blob Storage operations using Azurite as a local emulator.

## Solution Structure

| Project | Responsibility |
|---|---|
| `API` | Minimal API endpoints, DI setup, configuration |
| `Core` | Shared DTOs and domain exceptions |
| `Infrastructure` | `BlobService` implementation, interfaces, options |
| `Infrastructure.UnitTests` | Unit tests with NUnit and NSubstitute |

## Running Azurite

```bash
docker run -d \
  --name AzureBlobStorage \
  -p 10000:10000 \
  -v $(pwd)/.containers/blob_storage/data:/data \
  mcr.microsoft.com/azure-storage/azurite:latest \
  azurite-blob --blobHost 0.0.0.0 -l /data --skipApiVersionCheck
```

## Configuration

```json
{
  "CloudInfo": {
    "ContainerName": "your-container-name"
  }
}
```

## Running the API

```bash
dotnet run --project AzuriteBlobDemo.API
```

## Running Tests

```bash
dotnet test
```

## Endpoints

Base URL: `http://localhost:5203`

| Method | Route | Description |
|---|---|---|
| `POST` | `/files` | Upload a file (`multipart/form-data`) |
| `GET` | `/files` | List all files |
| `GET` | `/files/{fileId}` | Download a file by ID (UUID) |
| `DELETE` | `/files/{fileId}` | Delete a file by ID (UUID) |

## References

- [Azure Blob Storage quickstart for .NET](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=visual-studio%2Cmanaged-identity%2Croles-azure-portal%2Csign-in-azure-cli%2Cidentity-visual-studio&pivots=blob-storage-quickstart-scratch)
- [Stack Overflow \u2014 How to list all blobs in a container](https://stackoverflow.com/questions/32057636/how-to-get-a-list-of-all-the-blobs-in-a-container-in-azure)
- [YouTube \u2014 Azure Blob Storage with .NET walkthrough](https://youtu.be/Ft4SJgQETAk?si=-Wo4KwDXMVZdHNJM)
- [Docker Hub \u2014 Azurite image](https://hub.docker.com/r/microsoft/azure-storage-azurite)