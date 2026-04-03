namespace FoodGrabber.Shared.Abstractions;

public interface IObjectStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string? contentType = null, CancellationToken cancellationToken = default);
    Task<string> UploadAsync(byte[] fileBytes, string fileName, string? contentType = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default);
}
