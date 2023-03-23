using Dapper;

namespace AuthEvents.Data.DataAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<TModel>> LoadData<TModel>(
        string sql,
        DynamicParameters parameters,
        CancellationToken ct
    );

    Task<TResult> LoadScalar<TResult>(string sql, DynamicParameters parameters, CancellationToken ct);
    Task<TModel> SaveData<TModel>(string sql, DynamicParameters parameters, CancellationToken ct);
}