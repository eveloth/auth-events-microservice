using AuthEvents.Options;
using Serilog;

namespace AuthEvents.Installers;

public static class SerilogInstaller
{
    public static WebApplicationBuilder InstallSerilog(this WebApplicationBuilder builder)
    {
        var serilogOptions = new SerilogOptions();
        builder.Configuration.Bind(nameof(SerilogOptions), serilogOptions);

        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Conditional(
                _ => serilogOptions.EnableConsole,
                configuration => configuration.Console()
            )
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}