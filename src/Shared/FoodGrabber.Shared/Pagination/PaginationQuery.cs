namespace FoodGrabber.Shared.Pagination;

public sealed class PaginationQuery
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public int Page { get; init; } = DefaultPage;
    public int PageSize { get; init; } = DefaultPageSize;

    public int NormalizedPage => Page < 1 ? DefaultPage : Page;

    public int NormalizedPageSize
    {
        get
        {
            if (PageSize < 1)
            {
                return DefaultPageSize;
            }

            return PageSize > MaxPageSize ? MaxPageSize : PageSize;
        }
    }
}
