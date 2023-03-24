using AuthEvents.Contracts.Dto;
using AuthEvents.Contracts.Queries;
using AuthEvents.Contracts.Requests;
using AuthEvents.Contracts.Responses;
using AuthEvents.Domain;
using AuthEvents.Services;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<EventRequest> _validator;
        private readonly IEventService _eventService;

        public EventsController(
            ILogger<EventsController> logger,
            IMapper mapper,
            IValidator<EventRequest> validator,
            IEventService eventService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] PaginationQuery paginationQuery,
            CancellationToken ct,
            [FromQuery] TimeFilterQuery? timeFilter,
            [FromQuery] EventFilterQuery? eventFilter
        )
        {
            timeFilter =
                timeFilter?.StartDate is null & timeFilter?.EndDate is null ? null : timeFilter;

            eventFilter =
                eventFilter?.Id is null
                & eventFilter?.UserId is null
                & eventFilter?.EventType is null
                    ? null
                    : eventFilter;

            var events = await _eventService.Get(ct, timeFilter, eventFilter, paginationQuery);

            var eventsCount = await _eventService.Count(
                ct,
                timeFilter,
                eventFilter,
                paginationQuery
            );

            var response = new PagedResponse<EventDto>(
                _mapper.Map<IEnumerable<EventDto>>(events),
                paginationQuery,
                eventsCount
            );
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> IngressEvent(EventRequest request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var newEvent = _mapper.Map<Event>(request);
            var eventId = await _eventService.Add(newEvent, ct);

            _logger.LogInformation("Added new event ID {EventId}", eventId);

            return Ok(new CreatedResponse(eventId));
        }
    }
}