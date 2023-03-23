using AuthEvents.Data.Migrations;
using FluentMigrator.Runner;

namespace AuthEvents.Installers;

public static class MigrationsInstaller
{
    public static void InstallFluentMigrator(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddFluentMigratorCore()
            .ConfigureRunner(r =>
            {
                r.AddPostgres()
                    .WithGlobalConnectionString(
                        builder.Configuration.GetConnectionString("Postgres")
                    )
                    .ScanIn(typeof(InitialMigration).Assembly)
                    .For.Migrations();
            })
            .AddLogging(l => l.AddFluentMigratorConsole());
    }

    public static void RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }
}