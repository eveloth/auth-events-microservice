using AuthEvents.Options;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace AuthEvents.Data.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly ConnectionStringsOptions _connectionStrings;

    public SqlDataAccess(IOptions<ConnectionStringsOptions> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }

    public async Task<IEnumerable<TModel>> LoadData<TModel>(
        string sql,
        DynamicParameters parameters,
        CancellationToken ct
    )
    {
        await using var connection = new NpgsqlConnection(_connectionStrings.Postgres);

        return await connection.QueryAsync<TModel>(
                new CommandDefinition(sql, parameters, cancellationToken: ct)
            ) ?? Enumerable.Empty<TModel>();
    }

    public async Task<int> SaveData<TModel>(
        string sql,
        DynamicParameters parameters,
        CancellationToken ct
    )
    {
        await using var connection = new NpgsqlConnection(_connectionStrings.Postgres);

        return await connection.ExecuteAsync(
            new CommandDefinition(sql, parameters, cancellationToken: ct)
        );
    }
}