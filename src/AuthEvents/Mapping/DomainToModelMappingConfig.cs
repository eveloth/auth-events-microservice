using AuthEvents.Data;
using AuthEvents.Domain;
using Mapster;

namespace AuthEvents.Mapping;

public static class DomainToModelMappingConfig
{
    public static void ConfigureDomainToModelMapping(this IApplicationBuilder app)
    {
        TypeAdapterConfig<Event, EventModel>
            .NewConfig()
            .Map(dest => dest.EventType, src => src.EventType.ToString())
            .IgnoreNonMapped(false);
    }
}