using System.Text.Json.Serialization;

namespace AuthEvents.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventType
{
    SignIn = 0,
    SignOut = 1
}