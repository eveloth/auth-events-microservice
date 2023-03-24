using System.Text;

namespace AuthEvents.Data.Filters;

public class QueryBuilder
{
    private readonly StringBuilder _queryBuilder;
    //If any filter was applied, there should be 'and' after 'where'
    private bool _anyFilterApplied = false;
    //The same is with event filter
    private bool _eventFilterApplied = false;

    private QueryBuilder(string query)
    {
        _queryBuilder = new StringBuilder(query);
    }

    public static QueryBuilder Create(string query)
    {
        return new QueryBuilder(query);
    }

    public string ApplyFilters(
        TimeFilter? timeFilter,
        EventFilter? eventFilter,
        PaginationFilter? paginationFilter
    )
    {
        if (timeFilter is not null || eventFilter is not null)
        {
            _queryBuilder.Append(' ').Append("where");
            ApplyEventFilter(eventFilter);
            ApplyTimeFilter(timeFilter);
        }

        ApplyPaginationFilter(paginationFilter);
        return _queryBuilder.ToString();
    }

    private void ApplyTimeFilter(TimeFilter? filter)
    {
        if (filter is null)
            return;

        if (_anyFilterApplied)
        {
            _queryBuilder.Append(' ').Append("and");
        }

        if (filter.StartDate is not null && filter.EndDate is not null)
        {
            _queryBuilder.Append(' ').Append("time_fired > @StartDate and time_fired < @EndDate");
            _anyFilterApplied = true;
        }
        else if (filter.StartDate is not null)
        {
            _queryBuilder.Append(' ').Append("time_fired > @StartDate");
            _anyFilterApplied = true;
        }
        else if (filter.EndDate is not null)
        {
            _queryBuilder.Append(' ').Append("time_fired < @EndDate");
            _anyFilterApplied = true;
        }
    }

    private void ApplyEventFilter(EventFilter? filter)
    {
        if (filter is null)
        {
            return;
        }

        if (_anyFilterApplied)
        {
            _queryBuilder.Append(' ').Append("and");
        }

        if (filter.Id is not null)
        {
            _queryBuilder.Append(' ').Append("id = @Id");
            _eventFilterApplied = true;
            _anyFilterApplied = true;
        }

        if (filter.UserId is not null)
        {
            And();
            _queryBuilder.Append(' ').Append("user_id = @UserId");
            _eventFilterApplied = true;
            _anyFilterApplied = true;
        }

        if (filter.EventType is not null)
        {
            And();
            _queryBuilder.Append(' ').Append("event_type = @EventType");
            _eventFilterApplied = true;
            _anyFilterApplied = true;
        }

        void And()
        {
            if (_eventFilterApplied)
            {
                _queryBuilder.Append(' ').Append("and");
            }
        }
    }

    private void ApplyPaginationFilter(PaginationFilter? filter)
    {
        if (filter is null)
        {
            return;
        }

        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var take = filter.PageSize;
        var paginator = $"limit {take} offset {skip}";

        _queryBuilder.Append(' ').Append(paginator);
    }
}