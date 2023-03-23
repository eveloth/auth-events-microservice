using System.Text.Json;
using AuthEvents.Domain;

namespace AuthEvents.Contracts.Dto;

public record EventDto(long Id, Guid UserId, EventType EventType, DateTime TimeFired, JsonElement Payload);