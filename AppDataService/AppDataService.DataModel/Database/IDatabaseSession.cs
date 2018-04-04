using System.Collections.Generic;
using System.Data;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public interface IDatabaseSession
    {
        string User { get; }

        IDbConnection GetConnection();

        IEnumerable<T> Query<T>(string sql, IDictionary<string, object> parameterList);

        dynamic Insert<T>(string insertSql, T entity) where T : IDataModel;

        dynamic UpdateSingle<T>(string updateSql, IDictionary<string, object> parameterList, T entity) where T : IDataModel;
    }
}
