using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AuthEventsIntegration.Tests;

public class IntegrationTestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly IContainer _testcontainerDb;

    public IntegrationTestFactory()
    {
        _testcontainerDb = new ContainerBuilder()
            .WithImage("postgres:15")
            .WithEnvironment("POSTGRES_USER", "eventsdbuser")
            .WithEnvironment("POSTGRES_PASSWORD", "pass")
            .WithEnvironment("POSTGRES_DB", "events_db")
            .WithPortBinding(5555, 5432)
            .WithCleanUp(true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilCommandIsCompleted("sh", "-c", "sleep 4 && echo \"ready\"")
            )
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync() => await _testcontainerDb.StartAsync();

    public new async Task DisposeAsync() => await _testcontainerDb.StopAsync();
}