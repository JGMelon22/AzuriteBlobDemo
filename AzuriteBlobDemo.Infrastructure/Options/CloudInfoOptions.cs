namespace AzuriteBlobDemo.Infrastructure.Options;

public class CloudInfoOptions
{
    public const string SectionName = "CloudInfo";

    public string? ContainerName { get; set; }
}