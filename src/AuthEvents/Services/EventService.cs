using AuthEvents.Contracts.Queries;
using AuthEvents.Data;
using AuthEvents.Data.Filters;
using AuthEvents.Data.Repository;
using AuthEvents.Domain;
using Mapster;
using MapsterMapper;

namespace AuthEvents.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Event>> Get(
        CancellationToken ct,
        TimeFilterQuery? timeFilterQuery = null,
        EventFilterQuery? eventFilterQuery = null,
        PaginationQuery? paginationQuery = null
    )
    {
        var timeFilter = timeFilterQuery.Adapt<TimeFilterQuery?, TimeFilter?>();
        var eventFilter = eventFilterQuery.Adapt<EventFilterQuery?, EventFilter?>();
        var paginationFilter = paginationQuery.Adapt<PaginationQuery?, PaginationFilter?>();

        var result = await _eventRepository.Get(ct, timeFilter, eventFilter, paginationFilter);

        var events = _mapper.Map<IEnumerable<Event>>(result);
        return events;
    }

    public async Task<long> Add(Event newEvent, CancellationToken ct)
    {
        var eventModel = _mapper.Map<EventModel>(newEvent);

        return await _eventRepository.Add(eventModel, ct);
    }

    public async Task<long> Count(
        CancellationToken ct,
        TimeFilterQuery? timeFilterQuery = null,
        EventFilterQuery? eventFilterQuery = null,
        PaginationQuery? paginationQuery = null
    )
    {
        var timeFilter = timeFilterQuery.Adapt<TimeFilterQuery?, TimeFilter?>();
        var eventFilter = eventFilterQuery.Adapt<EventFilterQuery?, EventFilter?>();
        var paginationFilter = paginationQuery.Adapt<PaginationQuery?, PaginationFilter?>();

        var eventCount = await _eventRepository.Count(
            ct,
            timeFilter,
            eventFilter,
            paginationFilter
        );

        return eventCount;
    }
}