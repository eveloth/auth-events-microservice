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

        /// <summary>
        /// Returns events according to provided filters
        /// </summary>
        /// <response code="200">Returns events according to provided filters</response>
        /// <response code="400">If a validation error occured</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<EventDto>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetEvents(
            [FromQuery] PaginationQuery paginationQuery,
            CancellationToken ct,
            [FromQuery] TimeFilterQuery? timeFilter,
            [FromQuery] EventFilterQuery? eventFilter
        )
        {
            // It does the thing that I'd like model binding to do --
            // initializes object with null if there are no query parameters of the givent set
            // (represented by object fot convenience)
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

        /// <summary>
        /// Adds an event to the database
        /// </summary>
        /// <response code="200">Adds an event to the database</response>
        /// <response code="400">If a validation error occured</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
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