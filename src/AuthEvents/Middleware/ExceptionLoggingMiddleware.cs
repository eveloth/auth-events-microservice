using System.ComponentModel.DataAnnotations;
using Npgsql;

namespace AuthEvents.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;

    public ExceptionLoggingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionLoggingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e) when (e is NpgsqlException or PostgresException)
        {
            _logger.LogCritical("Error connecting to the database: {Error}", e.Message);
            context.Response.StatusCode = 500;
            throw;
        }
        catch (ValidationException e)
        {
            _logger.LogError("Validation error occured: {Error}", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Unknow error: {Error}", e.Message);
            context.Response.StatusCode = 500;
            throw;
        }
    }
}