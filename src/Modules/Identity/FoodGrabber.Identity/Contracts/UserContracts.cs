namespace FoodGrabber.Identity.Contracts
{
    public sealed record UserResponse(
        Guid UserId,
        string FullName,
        string Email,
        string Role
    );

    public sealed record CustomerResponse(
        Guid Id,
        string UserId,
        string FullName,
        string Email,
        string Address1,
        string Address2,
        string Phone1,
        string Phone2,
        string Image
        );

    public sealed record UserWithRoleDto(
        string Id,
        string FullName,
        string Email,
        string Role
    );

    public sealed record CustomerUpdateRequest(
        string Id,
        string FullName,
        string Email,
        string Address1,
        string Address2,
        string Phone1,
        string Phone2,
        string Image
        );



}
