using System.Runtime.Serialization;

namespace AzuriteBlobDemo.Core.Exceptions;

public class FileToLargeException : Exception
{
    public FileToLargeException()
    {
    }

    public FileToLargeException(string? message) : base(message)
    {
    }

    public FileToLargeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}