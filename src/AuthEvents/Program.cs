using System.Text.Json.Serialization;
using AuthEvents.Data.DataAccess;
using AuthEvents.Data.Repository;
using AuthEvents.Installers;
using AuthEvents.Mapping;
using AuthEvents.Options;
using AuthEvents.Services;
using AuthEvents.Validation;
using FluentValidation;
using MapsterMapper;
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
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

app.RunMigrations();

app.ConfigureRequestToDomainMapping();
app.ConfigureDomainToModelMapping();
app.ConfigureModelToDomainMapping();
app.ConfigureDomainToDtoMapping();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();