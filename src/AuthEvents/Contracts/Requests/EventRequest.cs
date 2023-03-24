using System.Text.Json;
using AuthEvents.Domain;

namespace AuthEvents.Contracts.Requests;

public record EventRequest
{
    public Guid UserId { get; init; }
    public EventType EventType { get; init; }
    public DateTime TimeFired { get; init; }
    public JsonElement Payload { get; init; }
};