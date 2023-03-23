using AuthEvents.Data.Filters;

namespace AuthEvents.Data.Repository;

public interface IEventRepository
{
    Task<IEnumerable<EventModel>> Get(
        CancellationToken ct,
        TimeFilter? timeFilter = null,
        EventFilter? eventFilter = null,
        PaginationFilter? paginationFilter = null
    );
    Task<long> Add(EventModel eventModel, CancellationToken ct);
    Task<int> Count(
        CancellationToken ct,
        TimeFilter? timeFilter = null,
        EventFilter? eventFilter = null,
        PaginationFilter? paginationFilter = null
    );
}