using AuthEvents.Domain;

namespace AuthEvents.Contracts.Queries;

public record EventFilterQuery(long? Id, Guid? UserId, EventType? EventType);