using Dapper;

namespace AuthEvents.Data.DataAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<TModel>> LoadData<TModel>(
        string sql,
        DynamicParameters parameters,
        CancellationToken ct
    );
    Task<int> SaveData<TModel>(string sql, DynamicParameters parameters, CancellationToken ct);
}