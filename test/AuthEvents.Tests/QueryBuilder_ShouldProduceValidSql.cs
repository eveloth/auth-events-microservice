using System.Diagnostics.CodeAnalysis;
using AuthEvents.Data.Filters;

namespace AuthEvents.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class QueryBuilder_ShouldProduceValidSql
{
    [Fact]
    public void ApplyFilters_1()
    {
        var tFilter = new TimeFilter
        {
            StartDate = new DateTime(2023, 1, 19),
            EndDate = new DateTime(2023, 3, 13)
        };
        var eFilter = new EventFilter { Id = 5006, EventType = "SignOut" };
        var pFilter = new PaginationFilter { PageNumber = 2, PageSize = 10 };

        var sut = QueryBuilder
            .Create("select * from event")
            .ApplyFilters(tFilter, eFilter, pFilter);
        const string expected =
            "select * from event where "
            + "id = @Id and event_type = @EventType "
            + "and time_fired > @StartDate "
            + "and time_fired < @EndDate "
            + "limit 10 offset 10";

        Assert.Equal(expected, sut);
    }

    [Fact]
    public void ApplyFilters_2()
    {
        TimeFilter? tFilter = null;
        var eFilter = new EventFilter { Id = 5006, EventType = "SignIn" };
        var pFilter = new PaginationFilter { PageNumber = 1, PageSize = 50 };

        var sut = QueryBuilder
            .Create("select * from event")
            .ApplyFilters(tFilter, eFilter, pFilter);
        const string expected =
            "select * from event where "
            + "id = @Id and event_type = @EventType "
            + "limit 50 offset 0";

        Assert.Equal(expected, sut);
    }

    [Fact]
    public void ApplyFilters_3()
    {
        var tFilter = new TimeFilter { EndDate = new DateTime(2019, 7, 24) };
        EventFilter? eFilter = null;
        PaginationFilter? pFilter = null;

        var sut = QueryBuilder
            .Create("select * from event")
            .ApplyFilters(tFilter, eFilter, pFilter);

        const string expected = "select * from event where " + "time_fired < @EndDate";

        Assert.Equal(expected, sut);
    }

    [Fact]
    public void ApplyFilters_4()
    {
        var sut = QueryBuilder.Create("select * from event").ApplyFilters(null, null, null);

        const string expected = "select * from event";

        Assert.Equal(expected, sut);
    }
}