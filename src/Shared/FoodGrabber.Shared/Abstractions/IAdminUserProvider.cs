namespace FoodGrabber.Shared.Abstractions;

public interface IAdminUserProvider
{
    Task<string?> GetDefaultAdminUserIdAsync(CancellationToken cancellationToken = default);
}
