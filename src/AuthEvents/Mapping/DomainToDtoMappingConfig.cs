using AuthEvents.Contracts.Dto;
using AuthEvents.Domain;
using AuthEvents.Extensions;
using Mapster;

namespace AuthEvents.Mapping;

public static class DomainToDtoMappingConfig
{
    public static void ConfigureDomainToDtoMapping(this IApplicationBuilder app)
    {
        TypeAdapterConfig<Event, EventDto>
            .NewConfig()
            .Map(dest => dest.Payload, src => src.Payload.ToJsonElement())
            .IgnoreNonMapped(false);
    }
}