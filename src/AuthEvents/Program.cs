using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AuthEvents.Data.DataAccess;
using AuthEvents.Data.Repository;
using AuthEvents.Data.Seeding;
using AuthEvents.Installers;
using AuthEvents.Mapping;
using AuthEvents.Middleware;
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

// Add Serilog with optional console sink
builder.InstallSerilog();

builder.Services.AddProblemDetails();

// Use Options pattern for managing connection strings
builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings)
);

// Configure and add fluent migrator to the DI container
builder.InstallFluentMigrator();

// Add services
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddSingleton<IMapper, Mapper>();

// Add seeder for testing environment
if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddScoped<Seeder>();
}

// Add validators using FluentValidation package
builder.Services.AddValidatorsFromAssemblyContaining<EventValidator>();

builder.Services
    .AddControllers(options =>
    {
        // Apply transformer defined below
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

if (app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    await seeder.PrepareDatabase();
}

// This is useful for reverse proxy setup
app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

// Default ASP.NET Core request logging is a bit too verbose; this provides us with simple informative logs
app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();
// Use exception logging middleware for better debugging experience
app.UseMiddleware<ExceptionLoggingMiddleware>();

app.MapControllers();

await app.RunAsync();

public partial class Program { }

// This transofmer transforms api routes to slug case, i.e. api/AccessControl -> api/access-control etc.
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