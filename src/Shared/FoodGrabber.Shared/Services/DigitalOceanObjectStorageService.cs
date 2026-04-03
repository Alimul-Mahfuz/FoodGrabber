using Amazon.S3;
using Amazon.S3.Model;
using FoodGrabber.Shared.Abstractions;

namespace FoodGrabber.Shared.Services;

public sealed class DigitalOceanObjectStorageService(
    IAmazonS3 s3Client,
    string bucketName,
    string publicBaseUrl) : IObjectStorageService
{
    private readonly IAmazonS3 _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    private readonly string _bucketName = string.IsNullOrWhiteSpace(bucketName)
        ? throw new ArgumentException("Bucket name is required.", nameof(bucketName))
        : bucketName.Trim();

    private readonly string _publicBaseUrl = string.IsNullOrWhiteSpace(publicBaseUrl)
        ? throw new ArgumentException("Public base url is required.", nameof(publicBaseUrl))
        : publicBaseUrl.TrimEnd('/');

    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name is required.", nameof(fileName));
        }

        var objectKey = BuildObjectKey(fileName);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = fileStream,
            AutoCloseStream = false,
            ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }

        await _s3Client.PutObjectAsync(request, cancellationToken);
        return $"{_publicBaseUrl}/{objectKey}";
    }

    public async Task<string> UploadAsync(
        byte[] fileBytes,
        string fileName,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileBytes);
        await using var stream = new MemoryStream(fileBytes);
        return await UploadAsync(stream, fileName, contentType, cancellationToken);
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        var objectKey = GetObjectKeyFromUrl(fileUrl);
        if (string.IsNullOrWhiteSpace(objectKey))
        {
            return;
        }

        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    private static string BuildObjectKey(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".bin";
        }

        return $"{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
    }

    private string GetObjectKeyFromUrl(string fileUrl)
    {
        var normalized = fileUrl.Trim();
        var prefix = $"{_publicBaseUrl}/";

        if (normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return normalized[prefix.Length..];
        }

        if (normalized.StartsWith(_publicBaseUrl, StringComparison.OrdinalIgnoreCase))
        {
            return normalized[_publicBaseUrl.Length..].TrimStart('/');
        }

        if (Uri.TryCreate(normalized, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.AbsolutePath.TrimStart('/');
        }

        return normalized.TrimStart('/');
    }
}
