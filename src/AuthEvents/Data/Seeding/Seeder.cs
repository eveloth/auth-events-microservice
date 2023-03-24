using AuthEvents.Data.Repository;

namespace AuthEvents.Data.Seeding;

public class Seeder
{
    private readonly IEventRepository _eventRepository;

    public Seeder(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task PrepareDatabase()
    {
        foreach (var eventModel in MockEvents.Events)
        {
            var eventId = await _eventRepository.Add(eventModel, CancellationToken.None);
        }
    }
}