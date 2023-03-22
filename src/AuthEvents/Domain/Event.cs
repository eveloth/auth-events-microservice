using System.Text.Json;

namespace AuthEvents.Domain;

public record Event
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public EventType EventType { get; set; }
    public DateTime TimeFired { get; set; }
    public JsonElement Payload { get; set; }
};