using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzuriteBlobDemo.Core.Exceptions;
using AzuriteBlobDemo.Infrastructure.Interfaces.Services;
using AzureBlobOptions = AzuriteBlobDemo.Infrastructure.Options;
using MicrosoftOptions = Microsoft.Extensions.Options;
using AzuriteBlobDemo.Infrastructure.Services;
using NSubstitute;

namespace AzuriteBlobDemo.Infrastructure.UnitTests.Services;

[TestFixture]
public class BlobServiceTests
{
    private IBlobService _blobService;
    private BlobServiceClient _blobServiceClient;
    private BlobContainerClient _blobContainerClient;
    private BlobClient _blobClient;

    [SetUp]
    public void SetUp()
    {
        _blobServiceClient = Substitute.For<BlobServiceClient>();
        _blobContainerClient = Substitute.For<BlobContainerClient>();
        _blobClient = Substitute.For<BlobClient>();

        var options =
            MicrosoftOptions.Options.Create(new AzureBlobOptions.CloudInfoOptions
                { ContainerName = "test-container" });

        _blobServiceClient.GetBlobContainerClient(Arg.Any<string>())
            .Returns(_blobContainerClient);

        _blobContainerClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(_blobClient);

        _blobService = new BlobService(_blobServiceClient, options);
    }

    [Test]
    public async Task Should_ThrowFileToLargeException_When_FileSizeExceedLimits()
    {
        // Arrange
        Stream stream = new MemoryStream(new byte[20 * 1024 * 1024]);

        // Act & Assert
        Assert.ThrowsAsync<FileToLargeException>(() => _blobService.UploadAsync(stream, "application/pdf"));

        // Assert
    }

    [Test]
    public async Task Should_UploadFile_WhenFileSizeIsWithLimit()
    {
        // Arrange
        Stream stream = new MemoryStream(new byte[1 * 1024 * 1024]);

        _blobClient
            .UploadAsync(Arg.Any<Stream>(), Arg.Any<BlobHttpHeaders>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Substitute.For<Response<BlobContentInfo>>());

        // Act
        Guid result = await _blobService.UploadAsync(stream, "application/pdf");

        // Assert
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
    }
}