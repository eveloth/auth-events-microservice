namespace AuthEventsIntegration.Tests;

public class IntegrationTests : IClassFixture<IntegrationTestFactory<Program>>
{
    private readonly IntegrationTestFactory<Program> _factory;

    public IntegrationTests(IntegrationTestFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestConnectivity()
    {
        var client = _factory.CreateClient();

        var respone = await client.GetAsync("api/events");
        respone.EnsureSuccessStatusCode();
    }
}