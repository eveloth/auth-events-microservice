using AuthEvents.Contracts.Queries;
using AuthEvents.Domain;

namespace AuthEvents.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> Get(
        CancellationToken ct,
        TimeFilterQuery? timeFilterQuery = null,
        EventFilterQuery? eventFilterQuery = null,
        PaginationQuery? paginationQuery = null
    );

    Task<long> Add(Event newEvent, CancellationToken ct);
    Task<long> Count(
        CancellationToken ct,
        TimeFilterQuery? timeFilterQuery = null,
        EventFilterQuery? eventFilterQuery = null,
        PaginationQuery? paginationQuery = null
    );
}