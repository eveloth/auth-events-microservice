using AuthEvents.Data;
using AuthEvents.Domain;
using Mapster;

namespace AuthEvents.Mapping;

public static class ModelToDomainMappingConfig
{
    public static void ConfigureModelToDomainMapping(this IApplicationBuilder app)
    {
        TypeAdapterConfig<EventModel, Event>
            .NewConfig()
            .Map(dest => dest.EventType, src => Enum.Parse<EventType>(src.EventType))
            .IgnoreNonMapped(false);
    }
}