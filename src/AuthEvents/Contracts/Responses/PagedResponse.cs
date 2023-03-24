using AuthEvents.Contracts.Queries;

namespace AuthEvents.Contracts.Responses;

public record PagedResponse<T>
{
    public PagedResponse()
    {

    }
    public PagedResponse(IEnumerable<T> data, PaginationQuery pagination, long entitesCount)
    {
        Data = data;
        PageNumber = pagination.PageNumber;
        PageSize = pagination.PageSize;
        Total = entitesCount;
    }

    public IEnumerable<T> Data { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public long Total { get; init; }
    public int PagesTotal => (int)Math.Ceiling((double)Total / PageSize);
}