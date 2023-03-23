using AuthEvents.Contracts.Requests;
using AuthEvents.Domain;
using Mapster;

namespace AuthEvents.Mapping;

public static class RequestToDomainMappingConfig
{
    public static void ConfigureRequestToDomainMapping(this IApplicationBuilder app)
    {
        TypeAdapterConfig<EventRequest, Event>
            .NewConfig()
            .Map(dest => dest.Payload, src => src.Payload.GetRawText())
            .IgnoreNonMapped(false);
    }
}