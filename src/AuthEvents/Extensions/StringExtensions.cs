using System.Text.Json;

namespace AuthEvents.Extensions;

public static class StringExtensions
{
    public static JsonElement ToJsonElement(this string json)
    {
        return JsonDocument.Parse(json).RootElement;
    }
}