namespace AuthEvents.Data.Filters;

public record TimeFilter
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
};