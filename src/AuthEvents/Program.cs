using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AuthEvents.Data.DataAccess;
using AuthEvents.Data.Repository;
using AuthEvents.Installers;
using AuthEvents.Mapping;
using AuthEvents.Options;
using AuthEvents.Services;
using AuthEvents.Validation;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;
using IMapper = MapsterMapper.IMapper;

var builder = WebApplication.CreateBuilder(args);

builder.InstallSerilog();

builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings)
);

builder.InstallFluentMigrator();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddSingleton<IMapper, Mapper>();

builder.Services.AddValidatorsFromAssemblyContaining<EventValidator>();

builder.Services
    .AddControllers(options =>
    {
        options.Conventions.Add(
            new RouteTokenTransformerConvention(new ToSlugCaseTransformerConvention())
        );
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

app.ConfigureMapping();
app.RunMigrations();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();

public partial class Program { }

public partial class ToSlugCaseTransformerConvention : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value is null
            ? null
            : ToSlugCaseTransformerRegex().Replace(value.ToString()!, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex ToSlugCaseTransformerRegex();
}