using AuthEvents.Contracts.Dto;
using AuthEvents.Contracts.Requests;
using AuthEvents.Data;
using AuthEvents.Domain;
using AuthEvents.Extensions;
using Mapster;

namespace AuthEvents.Mapping;

public static class MappingConfig
{
    public static void ConfigureMapping(this WebApplication app)
    {
        TypeAdapterConfig<EventModel, Event>
            .NewConfig()
            .Map(dest => dest.EventType, src => Enum.Parse<EventType>(src.EventType))
            .IgnoreNonMapped(false);
        TypeAdapterConfig<Event, EventDto>
            .NewConfig()
            .Map(dest => dest.Payload, src => src.Payload.ToJsonElement())
            .IgnoreNonMapped(false);
        TypeAdapterConfig<EventModel, Event>
            .NewConfig()
            .Map(dest => dest.EventType, src => Enum.Parse<EventType>(src.EventType))
            .IgnoreNonMapped(false);
        TypeAdapterConfig<EventRequest, Event>
            .NewConfig()
            .Map(dest => dest.Payload, src => src.Payload.GetRawText())
            .IgnoreNonMapped(false);
    }
}