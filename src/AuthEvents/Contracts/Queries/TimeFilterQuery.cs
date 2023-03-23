namespace AuthEvents.Contracts.Queries;

public record TimeFilterQuery(DateTime? StartDate, DateTime? EndDate);