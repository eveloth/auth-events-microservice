namespace AuthEvents.Data.Filters;

public record PaginationFilter
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}