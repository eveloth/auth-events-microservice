namespace AuthEvents.Data.Seeding;

public static class MockEvents
{
    public static List<EventModel> Events { get; } = new()
    {
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignIn",
            TimeFired = new DateTime(1917, 1, 13, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignIn",
            TimeFired = new DateTime(1920, 1, 13, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignIn",
            TimeFired = new DateTime(1930, 1, 13, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignIn",
            TimeFired = new DateTime(1930, 2, 15, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignIn",
            TimeFired = new DateTime(1944, 4, 15, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignOut",
            TimeFired = new DateTime(1944, 4, 15, 16, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignOut",
            TimeFired = new DateTime(1956, 9, 9, 4, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignOut",
            TimeFired = new DateTime(1997, 7, 24, 15, 25 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignOut",
            TimeFired = new DateTime(2001, 8, 6, 0, 0 ,0),
            Payload = "{\"device\":\"oneplus\"}"
        },
        new EventModel
        {
            UserId = Guid.NewGuid(),
            EventType = "SignOut",
            TimeFired = new DateTime(2023, 2, 17, 6, 0 ,30),
            Payload = "{\"device\":\"oneplus\"}"
        }
    };
}