namespace AuthEvents.Options;

public class ConnectionStringsOptions
{
    public const string ConnectionStrings = "ConnectionStrings";
    public string Postgres { get; set; } = default!;
}