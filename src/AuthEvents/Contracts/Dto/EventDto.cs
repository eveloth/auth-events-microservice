using System.Text.Json;
using AuthEvents.Domain;

namespace AuthEvents.Contracts.Dto;

public record EventDto
{
    public EventDto() { }

    public EventDto(
        long Id,
        Guid UserId,
        EventType EventType,
        DateTime TimeFired,
        JsonElement Payload
    )
    {
        this.Id = Id;
        this.UserId = UserId;
        this.EventType = EventType;
        this.TimeFired = TimeFired;
        this.Payload = Payload;
    }

    public long Id { get; init; }
    public Guid UserId { get; init; }
    public EventType EventType { get; init; }
    public DateTime TimeFired { get; init; }
    public JsonElement Payload { get; init; }
}