using FluentMigrator;

namespace AuthEvents.Data.Migrations;

[Migration(202303231000)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        const string sql =
            "create table if not exists event(" +
            "id bigserial primary key," +
            "user_id uuid not null," +
            "event_type varchar(63) not null," +
            "time_fired timestamp not null," +
            "payload jsonb not null);";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        Execute.Sql("drop table event");
    }
}