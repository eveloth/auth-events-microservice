using System.Text.Json.Serialization;
using AuthEvents.Domain;
using AuthEvents.Installers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.InstallSerilog();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();