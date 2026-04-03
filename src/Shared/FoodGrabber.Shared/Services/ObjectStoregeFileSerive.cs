using FoodGrabber.Shared.Abstractions;

namespace FoodGrabber.Shared.Services;

public sealed class ObjectStoregeFileSerive(string rootPath, string publicBaseUrl) : IObjectStorageService
{
    private readonly string _rootPath = string.IsNullOrWhiteSpace(rootPath)
        ? throw new ArgumentException("Root path is required.", nameof(rootPath))
        : rootPath.Trim();

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
        var absolutePath = Path.Combine(_rootPath, objectKey.Replace('/', Path.DirectorySeparatorChar));
        var directory = Path.GetDirectoryName(absolutePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var file = new FileStream(
            absolutePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true);

        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }

        await fileStream.CopyToAsync(file, cancellationToken);
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

    public Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Task.CompletedTask;
        }

        var objectKey = GetObjectKeyFromUrl(fileUrl);
        if (string.IsNullOrWhiteSpace(objectKey))
        {
            return Task.CompletedTask;
        }

        var absolutePath = Path.Combine(_rootPath, objectKey.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(absolutePath))
        {
            File.Delete(absolutePath);
        }

        return Task.CompletedTask;
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
