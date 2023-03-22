using AuthEvents.Data.DataAccess;
using AuthEvents.Data.Filters;
using Dapper;

namespace AuthEvents.Data.Repository;

public class EventRepository : IEventRepository
{
    private readonly ISqlDataAccess _db;

    public EventRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EventModel>> Get(
        CancellationToken ct,
        TimeFilter? timeFilter = null,
        EventFilter? eventFilter = null,
        PaginationFilter? paginationFilter = null
    )
    {
        var sql = QueryBuilder
            .Create("select * from event")
            .ApplyFilters(timeFilter, eventFilter, paginationFilter);

        var parameters = new DynamicParameters(timeFilter);
        parameters.AddDynamicParams(eventFilter);
        parameters.AddDynamicParams(paginationFilter);

        return await _db.LoadData<EventModel>(sql, parameters, ct);
    }

    public async Task<int> Add(EventModel eventModel, CancellationToken ct)
    {
        const string sql =
            @"insert into event
            (user_id, event_type,
            time_fired, payload)
            values
            (@UserId, @EventType,
            @TimeFired, @Payload::json)";

        var parameters = new DynamicParameters(eventModel);
        return await _db.SaveData<EventModel>(sql, parameters, ct);
    }
}