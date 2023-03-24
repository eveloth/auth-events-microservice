using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AuthEvents.Contracts.Dto;
using AuthEvents.Contracts.Requests;
using AuthEvents.Contracts.Responses;
using AuthEvents.Domain;
using AuthEvents.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

// ReSharper disable InconsistentNaming

namespace AuthEventsIntegration.Tests;

public class IntegrationTests_WithoutData : IClassFixture<IntegrationTestFactory<Program>>
{
    private readonly IntegrationTestFactory<Program> _factory;
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _serializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public IntegrationTests_WithoutData(IntegrationTestFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task TestConnectivity()
    {
        var response = await _client.GetAsync("api/events");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetEvents_ShouldReturn_DefaultPagination()
    {
        var response = await _client.GetAsync("api/events");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        events!.PageSize.Should().Be(50);
        events.PageNumber.Should().Be(1);
        events.PagesTotal.Should().Be(1);
    }

    [Fact]
    public async Task PostEvent_ShouldReturn_Ok()
    {
        var response = await _client.PostAsJsonAsync(
            "api/events",
            new EventRequest
            {
                UserId = Guid.NewGuid(),
                EventType = EventType.SignIn,
                TimeFired = new DateTime(2023, 3, 17),
                Payload = "{\"device\": \"oneplus\"}".ToJsonElement()
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var eventId = await JsonSerializer.DeserializeAsync<CreatedResponse>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );
    }

    [Fact]
    public async Task GetEvent_ShouldReturn_Count8()
    {
        var query = new Dictionary<string, string?> { ["PageSize"] = "8" };
        var uri = QueryHelpers.AddQueryString("api/events", query);

        var response = await _client.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        events!.Data.Should().HaveCount(8);
        events.PageSize.Should().Be(8);
        events.PageNumber.Should().Be(1);
        events.PagesTotal.Should().Be(2);
    }

    [Fact]
    public async Task GetEvent_SignOutCount_ShouldBe5()
    {
        var query = new Dictionary<string, string?> { ["EventType"] = "SignOut" };
        var uri = QueryHelpers.AddQueryString("api/events", query);

        var response = await _client.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        events!.Data.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetEvent_TestDateRanges_1()
    {
        var query = new Dictionary<string, string?>
        {
            ["StartDate"] = "1917-1-1",
            ["EndDate"] = "1950-1-1"
        };
        var uri = QueryHelpers.AddQueryString("api/events", query);

        var response = await _client.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        events!.Data.Should().HaveCount(6);
    }

    [Fact]
    public async Task GetEvent_TestDateRanges_2()
    {
        var query = new Dictionary<string, string?> { ["StartDate"] = "2023-2-18" };
        var uri = QueryHelpers.AddQueryString("api/events", query);

        var response = await _client.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        Assert.True(events!.Data.Count() <= 1);
    }

    [Fact]
    public async Task GetEvent_TestDateRanges_3()
    {
        var query = new Dictionary<string, string?> { ["EndDate"] = "1944-4-15T15:00:00" };
        var uri = QueryHelpers.AddQueryString("api/events", query);

        var response = await _client.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var events = await JsonSerializer.DeserializeAsync<PagedResponse<EventDto>>(
            await response.Content.ReadAsStreamAsync(),
            _serializerOptions
        );

        events.Should().NotBeNull();
        events!.Data.Should().HaveCount(5);
    }
}