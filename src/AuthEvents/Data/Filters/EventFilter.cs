namespace AuthEvents.Data.Filters;

public record EventFilter
{
    public long? Id { get; init; }
    public Guid? UserId { get; init; }
    public string? EventType { get; init; }
}