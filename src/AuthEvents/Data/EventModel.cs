namespace AuthEvents.Data;

public record EventModel
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string EventType { get; set; } = default!;
    public DateTime TimeFired { get; set; }
    public string Payload { get; set; } = default!;
};